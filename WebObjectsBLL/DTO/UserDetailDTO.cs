using WebLoginBLL.DTO;
using WebObjectsBLL.DTO;

namespace WebObjectsBLL.DTO
{
    public class UserDetailDTO
    {
        public UserDTO User { get; set; } = null!;
        public ClientDetailDTO? Client { get; set; }
    }
}
