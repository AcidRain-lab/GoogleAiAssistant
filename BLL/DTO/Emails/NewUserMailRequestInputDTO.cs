namespace BLL.DTO.Emails
{
    public class NewUserMailRequestInputDTO
    {
        /// <summary>
        /// Get or set the email address
        /// </summary>
        public string ToEmail { get; set; } = string.Empty;
        /// <summary>
        /// Get or set the user name
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// Get or set the password 
        /// </summary>
        public string Password { get; set; } = string.Empty;
        /// <summary>
        /// Get or set the password reset token
        /// </summary>
        public string PasswordResetToken { get; set; } = string.Empty;
        /// <summary>
        /// Get or set the call back url
        /// </summary>
        public string CallBackUrl { get; set; }=string.Empty;
    }
}
