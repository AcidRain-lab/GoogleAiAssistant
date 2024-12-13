namespace BLL.DTO.Emails
{
    public class NewUserMailRequestOutputDTO
    {
        public string ErrorMessage { get; set; } = String.Empty;
        public bool IsError { get; set; }
    }
}
