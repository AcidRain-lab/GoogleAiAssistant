namespace BLL.DTO.Users
{
    public class UserListForDropdownDTO
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Base64Image { get; set; } = string.Empty;
    }
}
