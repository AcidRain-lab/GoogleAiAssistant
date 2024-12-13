using System.ComponentModel.DataAnnotations;

namespace WebLoginBLL.DTO
{
    public class RoleDTO
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide a role name.")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
