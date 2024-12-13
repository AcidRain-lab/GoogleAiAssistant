using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class UserDTO : AvatarDTO
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Please enter username.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter password.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter email address."), EmailAddress(ErrorMessage = "Please enter valid email address.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter phone."), RegularExpression(@"^\d{3} \d{3} \d{4}$", ErrorMessage = "Please enter valid phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please enter first name.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter last name.")]
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Please select role.")]
        public int RoleId { get; set; }

        public string? RoleName { get; set; }

        public string? Initials { get; set; }

        public bool HasAvatar { get; set; }
        public Guid? ResetPasswordToken { get; set; } = Guid.Empty;
        public UserDTO(User user)
        {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            Phone = user.Phone ?? "";
            FirstName = user.FirstName;
            LastName = user.LastName;
            IsActive = user.IsActive;
            RoleId = user.RoleId;
            Initials = user.Initials;
            RoleName = user.Role?.Name;
            HasAvatar = user.HasAvatar;

           
        }

        public UserDTO()
        {

        }

    }
}