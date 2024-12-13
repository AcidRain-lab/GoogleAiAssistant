using BLL.DTO.Common;
using BLL.DTO.Emails;
namespace BLL.Services
{
    public interface IEmailManager
    {
        Task<NewUserMailRequestOutputDTO> SendWelcomeMailToNewUserAddedByAdmin(NewUserMailRequestInputDTO input);
        Task<EmailMessagesInputDTO> GetMailMessagesAsync(MailMessagesRequestInputDTO input);
        Task<MailMessagesDTO> GetMessageByUniqueId(MailMessagesRequestInputDTO input);
        Task<MailMessagesDTO> GetMessageByMessageId(MailMessagesRequestInputDTO input);
        Task<NewUserMailRequestOutputDTO> SendEmailToMultipleUsers(MailRequestInputDTO input);
        Task<ServiceResult> DeleteEmails(MailMessagesRequestInputDTO input);
    }
}
