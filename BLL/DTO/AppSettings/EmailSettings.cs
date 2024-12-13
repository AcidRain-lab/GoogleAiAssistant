namespace BLL.DTO.AppSettings
{
    public class EmailSettings
    {
        public string SenderEmailId { get; set; } = string.Empty;
        public string SenderDisplayName { get; set; } = string.Empty;
        public string SmtpPassword { get; set; } = string.Empty;
        public string SmtpHost { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
    }
}
