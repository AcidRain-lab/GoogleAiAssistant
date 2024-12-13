using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Auths
{
    public class ResetPasswordInputDTO
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirm password must match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
        public string? Token { get; set; }
    }
}
