namespace BLL.DTO.Emails
{
    public class MailRequestCredParamsDTO
    {
        public string EmailSmtpUserName { get; set; }   =string.Empty;
        public string EmailSmtpPassword { get; set; } =string.Empty;
        public bool EmailUseSSL { get; set; } = true;
        public int EmailSmtpPort { get; set; } = 993;
        public string EmailSmtpServer { get; set; } = string.Empty;
    }
}
