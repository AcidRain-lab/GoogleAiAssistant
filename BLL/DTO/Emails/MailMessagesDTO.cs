
namespace BLL.DTO.Emails
{
    public class MailMessagesDTO
    {
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
        public string SelectedFolder { get; set; } = string.Empty;
        public string MessageUniqueId { get; set; } = string.Empty;
        public string MessageId { get; set; } = string.Empty;
        public string Base64Image { get; set; } = string.Empty;
        public List<MailMessagesExt> ReplyList { get; set; } = new();
        public List<EmailAttachmentDTO> Attachments { get; set; } = new();
    }
    public class MailMessagesExt
    {
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string ResentDate { get; set; } = string.Empty;
        public string ResentFrom { get; set; } = string.Empty;
    }
}
