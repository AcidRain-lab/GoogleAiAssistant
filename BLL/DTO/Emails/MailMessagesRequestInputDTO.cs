namespace BLL.DTO.Emails
{
    public class MailMessagesRequestInputDTO
    {
        public int EmailSmtpPort { get; set; }
        public string EmailSmtpServer { get; set; } = string.Empty;
        public string EmailSmtpUserName { get; set; } = string.Empty;
        public string EmailSmtpPassword { get; set; } = string.Empty;
        public bool EmailUseSSL { get; set; } = true;
        public string SelectedFolder { get; set; } = string.Empty;
        public string MessageUniqueId { get; set; } = string.Empty;
        public string MessageId { get; set; } = string.Empty;
        public List<string> MessageIds { get; set; }=new();
        public Guid UserId { get; set; }
    }
}
