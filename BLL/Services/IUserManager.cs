
using BLL.DTO.Auths;
using BLL.DTO.Common;
using BLL.DTO.Users;

namespace BLL.Services
{
    public interface IUserManager
    {
        Task<ServiceResult> IsResetPasswordTokenValid(string token);
        Task<ServiceResult> ResetUserPasswordByToken(ResetPasswordInputDTO input);
        Task<UserSummaryDTO> GetUserDetailByToken(Guid token);
        Task<List<UserListForDropdownDTO>> GetUsersList();
        Task<UserSummaryDTO> GetUserDetailById(Guid id);

    }
}
