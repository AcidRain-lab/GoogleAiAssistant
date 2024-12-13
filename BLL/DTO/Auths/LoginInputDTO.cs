
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Auths
{
    public class LoginInputDTO
    {
        [Required(ErrorMessage = "User name is required.")]
        public string UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; } = false;
    }
}
