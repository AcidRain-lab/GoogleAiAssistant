using System.ComponentModel.DataAnnotations;

namespace WebLoginBLL.DTO
{
    public class UserDTO
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Please provide a username.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please provide a password.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please provide an email address.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please provide a phone number.")]
        public string Phone { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public int RoleId { get; set; }

        public string? RoleName { get; set; }
    }
}
