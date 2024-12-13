
using Microsoft.AspNetCore.Http;

namespace BLL.DTO.Emails
{
    public class MailRequestInputDTO
    {
        public string? ToEmail { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public string? From { get; set; }
        public string? BccEmail { get; set; } = string.Empty;
        public string? CcEmail { get; set; } = string.Empty;
        public List<string>? ToEmails { get; set; }
        
        public List<IFormFile>? Attachments
        {
            get; set;
        }
        public MailRequestCredParamsDTO? ServerCredentials { get; set; }
    }
}
