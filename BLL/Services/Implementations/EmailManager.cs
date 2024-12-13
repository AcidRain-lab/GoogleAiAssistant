using BLL.DTO.AppSettings;
using BLL.DTO.Emails;
using MailKit.Net.Imap;
using MailKit;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Search;
using DAL.Repositories;
using System.Text.RegularExpressions;
using System;
using BLL.DTO.Common;

namespace BLL.Services.Implementations
{
    public class EmailManager : IEmailManager
    {
        private readonly IAvatarRepository _avatarRepository;
        private readonly EmailSettings _emailSetting;
        private readonly IContactRepository _contactRepository;
        public EmailManager(IOptions<EmailSettings> emailSetting, IAvatarRepository avatarRepository, IContactRepository contactRepository)
        {
            _emailSetting = emailSetting.Value;
            _avatarRepository = avatarRepository;
            _contactRepository = contactRepository;
        }
        /// <summary>
        /// Mail to inform new user added by Admin
        /// </summary>
        /// <param name="input">Model</param> 
        /// <summary> 
        public async Task<NewUserMailRequestOutputDTO> SendWelcomeMailToNewUserAddedByAdmin(NewUserMailRequestInputDTO input)
        {
            try
            {
                var message = "Hi,</br>" +
                "We are excited to welcome you to the team! We are sharing your login detail by using that you can login to our portal.<br/>" +
                "Login Email :" + input.ToEmail +
                "<br/>Login Password :" + input.Password +
                "<br/>Or to reset your password please" + $" <a href='{input.CallBackUrl}'>click here.</a>";
                //Send Email 
                await SendEmail(new MailRequestInputDTO()
                {

                    Subject = "Your Login Detail",
                    ToEmail = input.ToEmail,
                    Body = message
                });
                return new NewUserMailRequestOutputDTO() { IsError = false, ErrorMessage = string.Empty };
            }
            catch (Exception ex)
            {
                return new NewUserMailRequestOutputDTO()
                {
                    IsError = true,
                    ErrorMessage = ex.Message
                };

            }

        }
        public async Task<NewUserMailRequestOutputDTO> SendEmailToMultipleUsers(MailRequestInputDTO input)
        {
            try
            {
                await SendEmails(input);
                return new NewUserMailRequestOutputDTO() { IsError = false, ErrorMessage = string.Empty };
            }
            catch (Exception ex)
            {
                return new NewUserMailRequestOutputDTO()
                {
                    IsError = true,
                    ErrorMessage = ex.Message
                };

            }
        }
        public async Task<EmailMessagesInputDTO> GetMailMessagesAsync(MailMessagesRequestInputDTO input)
        {
            EmailMessagesInputDTO emailMessagesInputDTO = new();
            using (var client = new ImapClient())
            {
                await client.ConnectAsync(input.EmailSmtpServer, input.EmailSmtpPort, input.EmailUseSSL);
                await client.AuthenticateAsync(input.EmailSmtpUserName, input.EmailSmtpPassword);
                var inbox = client.Inbox;
                await inbox.OpenAsync(FolderAccess.ReadOnly);
                emailMessagesInputDTO.Inbox = GetInboxMessages(client);
                emailMessagesInputDTO.Sent = await GetSentMessages(client);
                emailMessagesInputDTO.Draft = await GetDraftMessages(client);
                await client.DisconnectAsync(true);
            }
            return emailMessagesInputDTO;
        }
        public async Task<MailMessagesDTO> GetMessageByUniqueId(MailMessagesRequestInputDTO input)
        {
            UniqueId uniqueId = ParseStringToUniqueId(input.MessageUniqueId);
            MailMessagesDTO selectedMessage = new();
            using (var client = new ImapClient())
            {
                await client.ConnectAsync(input.EmailSmtpServer, input.EmailSmtpPort, input.EmailUseSSL);
                await client.AuthenticateAsync(input.EmailSmtpUserName, input.EmailSmtpPassword);
                var selectedMessageFolder = client.GetFolder(input.SelectedFolder);
                await selectedMessageFolder.OpenAsync(FolderAccess.ReadOnly);
                var message = selectedMessageFolder.GetMessage(uniqueId);
                selectedMessage.Subject = message.Subject;
                var from = message.From;
                if (from != null && from.Count > 0)
                {
                    var firstFrom = from[0].ToString();
                    selectedMessage.From = ExtractEmailAddress(firstFrom);
                    selectedMessage.FromName = ExtractName(firstFrom);
                }
                selectedMessage.Date = message.Date.DateTime.ToString();
                selectedMessage.Body = GetEmailMessageBody(message);
                if (!string.IsNullOrEmpty(selectedMessage.From))
                {
                    var contact = await _contactRepository.GetContactByEmail(selectedMessage.From);
                    if (contact != null)
                    {
                        var avatar = await _avatarRepository.GetAvatarByAssociatedRecordId(contact.Id);

                        if (avatar != null)
                        {
                            selectedMessage.Base64Image = Convert.ToBase64String(avatar.Content);
                        }
                    }
                }
                //attachments
                selectedMessage.Attachments = GetAttachments(message);

                var uniqueIds = new List<UniqueId>();
                uniqueIds.Add(uniqueId);
                foreach (var detail in selectedMessageFolder.Fetch(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.References))
                {
                    if (detail.References.Contains(input.MessageId))
                    {
                        uniqueIds.Add(detail.UniqueId);
                    }
                }
                List<MailMessagesExt> messagesWithReply = new();
                foreach (var id in uniqueIds)
                {
                    var childMessage = selectedMessageFolder.GetMessage(id);
                    messagesWithReply.Add(new MailMessagesExt()
                    {
                        Body = GetEmailMessageBody(childMessage),
                        Subject = childMessage.Subject,
                        ResentDate = childMessage.ResentDate.ToString(),
                        ResentFrom = childMessage.ResentFrom.ToString(),
                    });
                }
                selectedMessage.ReplyList = messagesWithReply;
                await client.DisconnectAsync(true);
            }

            return selectedMessage;
        }

        public async Task<MailMessagesDTO> GetMessageByMessageId(MailMessagesRequestInputDTO input)
        {
            MailMessagesDTO selectedMessage = new();
            using (var client = new ImapClient())
            {
                await client.ConnectAsync(input.EmailSmtpServer, input.EmailSmtpPort, input.EmailUseSSL);
                await client.AuthenticateAsync(input.EmailSmtpUserName, input.EmailSmtpPassword);
                SpecialFolder selectedFolder = SpecialFolder.All;
                if (input.SelectedFolder == "Sent")
                {
                    selectedFolder = SpecialFolder.Sent;
                }
                else if (input.SelectedFolder == "Drafts")
                {
                    selectedFolder = SpecialFolder.Drafts;
                }
                var sentMessageFolder = client.GetFolder(selectedFolder);
                await sentMessageFolder.OpenAsync(FolderAccess.ReadOnly);
                var query = SearchQuery.HeaderContains("Message-Id", input.MessageId);
                var uids = sentMessageFolder.Search(query);

                if (uids.Count == 0)
                {
                    selectedMessage.Body = "";
                    return selectedMessage;
                }

                var message = sentMessageFolder.GetMessage(uids[0]);
                selectedMessage.MessageId = message.MessageId;
                selectedMessage.Body = GetEmailMessageBody(message);
                selectedMessage.Attachments = GetAttachments(message);
                await client.DisconnectAsync(true);
            }
            return selectedMessage;
        }
       public async Task<ServiceResult> DeleteEmails(MailMessagesRequestInputDTO input)
        {

            using (var client = new ImapClient())
            {
                await client.ConnectAsync(input.EmailSmtpServer, input.EmailSmtpPort, input.EmailUseSSL);
                await client.AuthenticateAsync(input.EmailSmtpUserName, input.EmailSmtpPassword);
                var messageFolder = client.Inbox;
                if (input.SelectedFolder == "Sent")
                {
                    messageFolder = client.GetFolder(SpecialFolder.Sent);
                }
                else if (input.SelectedFolder == "Drafts")
                {
                    messageFolder = client.GetFolder(SpecialFolder.Drafts);
                }
                await messageFolder.OpenAsync(FolderAccess.ReadWrite);
                foreach (var messageId in input.MessageIds)
                {
                    var uids = await messageFolder.SearchAsync(SearchQuery.HeaderContains("Message-Id", messageId));
                    foreach (var uid in uids)
                    {
                        var message = await messageFolder.GetMessageAsync(uid);
                        await messageFolder.AddFlagsAsync(uid, MessageFlags.Deleted, true);
                        await messageFolder.ExpungeAsync();
                    }
                }
                await client.DisconnectAsync(true);
                return new ServiceResult(ServiceResultStatus.Success, "Emails deleted successfully.");
            }

        }
        #region Private Methods
        /// <summary>
        /// Send Email By MailKit SMTP
        /// </summary>
        /// <param name="mailRequest">Model</param>
        /// <returns></returns>
        private async Task<string> SendEmail(MailRequestInputDTO mailRequest)
        {
            var message = "";
            MimeMessage email = new()
            {
                Sender = MailboxAddress.Parse(_emailSetting.SenderEmailId)
            };
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                smtp.Connect(_emailSetting.SmtpHost, _emailSetting.SmtpPort, SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailSetting.SenderEmailId, _emailSetting.SmtpPassword);
                message = await smtp.SendAsync(email);

                smtp.Disconnect(true);
            }
            return message;
        }

        private async Task<string> SendEmails(MailRequestInputDTO mailRequest)
        {
            var message = "";
            MimeMessage email = new()
            {
                Sender = MailboxAddress.Parse(mailRequest.From),

            };
            foreach (var toEmail in mailRequest.ToEmails)
            {
                email.To.Add(MailboxAddress.Parse(toEmail));
            }
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                smtp.Connect(mailRequest.ServerCredentials.EmailSmtpServer, mailRequest.ServerCredentials.EmailSmtpPort, SecureSocketOptions.StartTls);
                smtp.Authenticate(mailRequest.ServerCredentials.EmailSmtpUserName, mailRequest.ServerCredentials.EmailSmtpPassword);
                message = await smtp.SendAsync(email);

                smtp.Disconnect(true);
            }
            return message;
        }
        private List<MailMessagesDTO> GetInboxMessages(ImapClient client)
        {
            var messages = new List<MailMessagesDTO>();
            //
            var inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadOnly);
            var messageSummaries = inbox.Fetch(0, -1, MessageSummaryItems.Full | MessageSummaryItems.UniqueId)
                                                    .OrderByDescending(summary => summary.Date)
                                                    .ToList();
            foreach (var summary in messageSummaries)
            {
                var message = inbox.GetMessage(summary.UniqueId);
                var inboxMessage = new MailMessagesDTO
                {
                    Subject = message.Subject,
                    From = message.From.ToString(),
                    Date = message.Date.DateTime.ToString(),
                    Body = GetEmailMessageBody(message),
                    MessageUniqueId = summary.UniqueId.ToString(),
                    SelectedFolder = "INBOX",
                    MessageId = message.MessageId,
                };

                messages.Add(inboxMessage);
            }
            return messages;
        }

        private async Task<List<MailMessagesDTO>> GetSentMessages(ImapClient client)
        {
            var messages = new List<MailMessagesDTO>();
            var sentMessageFolder = client.GetFolder(SpecialFolder.Sent);
            sentMessageFolder.Open(FolderAccess.ReadOnly);
            var uids = sentMessageFolder.Search(SearchQuery.All).OrderByDescending(x => sentMessageFolder.GetMessage(x).Date);
            foreach (var uid in uids)
            {
                var message = await sentMessageFolder.GetMessageAsync(uid);
                var sentMessage = new MailMessagesDTO
                {
                    Subject = message.Subject,
                    From = message.From.ToString(),
                    Date = message.Date.DateTime.ToString(),
                    Body = GetEmailMessageBody(message),
                    MessageUniqueId = uid.ToString(),
                    SelectedFolder = "Sent",
                    MessageId = message.MessageId,
                };
                messages.Add(sentMessage);
            }
            return messages;
        }

        private async Task<List<MailMessagesDTO>> GetDraftMessages(ImapClient client)
        {
            var messages = new List<MailMessagesDTO>();
            var draftsMessageContainer = client.GetFolder(SpecialFolder.Drafts);
            draftsMessageContainer.Open(FolderAccess.ReadOnly);
            var uids = draftsMessageContainer.Search(SearchQuery.All).OrderByDescending(x => draftsMessageContainer.GetMessage(x).Date);
            foreach (var uid in uids)
            {
                var message = await draftsMessageContainer.GetMessageAsync(uid);
                var draftMessage = new MailMessagesDTO
                {
                    Subject = message.Subject,
                    From = message.From.ToString(),
                    Date = message.Date.DateTime.ToString(),
                    Body = GetEmailMessageBody(message),
                    MessageUniqueId = uid.ToString(),
                    SelectedFolder = "Drafts",
                    MessageId = message.MessageId,
                };
                messages.Add(draftMessage);
            }
            return messages;
        }
        private string GetEmailMessageBody(MimeMessage message)
        {
            string bodyText = string.Empty;
            if (message.Body is TextPart textPart)
            {
                bodyText = textPart.Text;
            }
            else if (message.Body is MimeKit.Multipart multipart)
            {
                foreach (var part in multipart)
                {
                    if (part is TextPart text)
                    {
                        if (text.ContentType.MediaType == "text/plain" || text.ContentType.MediaType == "text")
                        {
                            bodyText = text.Text;
                        }
                        else if (text.ContentType.MediaType == "text/html")
                        {
                            bodyText = text.Text;
                        }
                    }
                }
            }
            bodyText.Replace("\r\n", string.Empty);
            return bodyText;
        }


        private UniqueId ParseStringToUniqueId(string messageId)
        {
            if (!UniqueId.TryParse(messageId, out UniqueId uniqueId))
            {
                throw new ArgumentException("Invalid format.");
            }

            return uniqueId;
        }

        private string ExtractName(string fromString)
        {
            string pattern = "\"(?<name>[^\"]+)\"";
            string name = string.Empty;
            Match match = Regex.Match(fromString, pattern);
            if (match.Success)
            {
                Group nameGroup = match.Groups["name"];
                name = nameGroup.Value;
            }
            return name;
        }
        private string ExtractEmailAddress(string fromString)
        {
            string pattern = @"<(?<email>[^<>]+)>";
            string emailAddress = string.Empty;
            Match match = Regex.Match(fromString, pattern);

            if (match.Success)
            {
                Group emailGroup = match.Groups["email"];
                emailAddress = emailGroup.Value;
            }
            return emailAddress;
        }
        private List<EmailAttachmentDTO> GetAttachments(MimeMessage message)
        {
            var attachments = new List<EmailAttachmentDTO>();

            foreach (var bodyPart in message.BodyParts)
            {
                if (bodyPart is MimePart mimePart)
                {
                    if (!string.IsNullOrEmpty(mimePart.FileName))
                    {
                        var attachment = new EmailAttachmentDTO
                        {
                            FileName = mimePart.FileName
                        };
                        using (var memoryStream = new MemoryStream())
                        {
                            mimePart.Content.DecodeTo(memoryStream);
                            attachment.Content = memoryStream.ToArray();
                        }
                        if (attachment.Content != null)
                        {
                            attachment.Base64Content = Convert.ToBase64String(attachment.Content);
                            attachment.MimeType = mimePart.ContentType.MimeType;
                        }
                        attachments.Add(attachment);
                    }
                }
            }

            return attachments;
        }
        #endregion
    }
}
