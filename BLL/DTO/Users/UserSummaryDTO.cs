namespace BLL.DTO.Users
{
    public class UserSummaryDTO
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string EmailSmtpPassword { get; set; } = string.Empty;
        public string EmailSmtpServer { get; set; } = string.Empty;
        public int EmailSmtpPort { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
