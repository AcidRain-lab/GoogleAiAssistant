using BLL.Constants;
using BLL.DTO;
using BLL.DTO.Auths;
using BLL.DTO.Common;
using BLL.DTO.Users;
using WebAuthCoreBLL.Helpers;
using DAL.Repositories;

namespace BLL.Services.Implementations
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IAvatarRepository _avatarRepository;
        public UserManager(IUserRepository userRepository, IAvatarRepository avatarRepository)
        {
            _userRepository = userRepository;
            _avatarRepository = avatarRepository;
        }

        public async Task<UserSummaryDTO> GetUserDetailByToken(Guid token)
        {
            var user = await _userRepository.GetUserByResetPasswordToken(token);
            return new UserSummaryDTO()
            {
                UserName = user.UserName,
                Email = user.Email
            };
        }

        public async Task<ServiceResult> IsResetPasswordTokenValid(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return new ServiceResult(ServiceResultStatus.Error, "Reset password token is required.");
            }
            bool tokenExists = await _userRepository.IsResetPasswordTokenValid(Guid.Parse(token));
            if (tokenExists)
            {
                return new ServiceResult(ServiceResultStatus.Success);
            }
            return new ServiceResult(ServiceResultStatus.NotFound, "Reset password link is expired.");
        }

        public async Task<ServiceResult> ResetUserPasswordByToken(ResetPasswordInputDTO input)
        {
            if (string.IsNullOrEmpty(input.Token))
            {
                return new ServiceResult(ServiceResultStatus.Error, "To reset password token is required.");
            }
            var user = await _userRepository.GetUserByResetPasswordToken(Guid.Parse(input.Token));
            if (user == null)
            {
                return new ServiceResult(ServiceResultStatus.NotFound, "User not exists.");
            }
            user.Password = input.Password;
            user.PasswordHash = SecurePasswordHasher.Hash(user.Password);
            user.ResetPasswordToken = Guid.NewGuid();
            int updateStatus = await _userRepository.Update(user);
            if (updateStatus == 0)
            {
                return new ServiceResult(ServiceResultStatus.Error, "Something went wrong while reseting the password. Please try after some time.");
            }
            return new ServiceResult(ServiceResultStatus.Success, "Password reset successfully");
        }

        public async Task<List<UserListForDropdownDTO>> GetUsersList()
        {
            var usersList = await _userRepository.GetAll();
            var avatarList = await _avatarRepository.GetAll();
            List<UserListForDropdownDTO> users = new();
            if (usersList.Any())
            {
                usersList.ForEach(user =>
                {
                    users.Add(new UserListForDropdownDTO()
                    {
                        UserId = user.Id,
                        Email = user.Email,
                        UserName = user.UserName,
                    });
                });
            }
            if (users.Any())
            {
                if (avatarList.Any())
                {
                    users.ForEach(user =>
                    {
                        if (avatarList.Any(x => x.AssociatedRecordId == user.UserId))
                        {
                            var content = avatarList.FirstOrDefault(x => x.AssociatedRecordId == user.UserId)!.Content;
                            if (content != null)
                            {
                                user.Base64Image = Convert.ToBase64String(content);
                            }
                        }
                    });
                }

            }
            return users;
        }
        public async Task<UserSummaryDTO> GetUserDetailById(Guid id)
        {
            var user = await _userRepository.GetById(id);
            UserSummaryDTO summary = new();
            if (user == null)
            {
                summary.ErrorMessage = "LoggedIn user with  valid EmailPassword, EmailSmtpServer and EmailSmtpPort can view the email messages.";
                return summary;
            }

            summary.UserName = user.UserName;
            summary.Email = user.Email;
            if (!string.IsNullOrEmpty(user.EmailPassword) && !string.IsNullOrEmpty(user.EmailSmtpServer) && user.EmailSmtpPort != null)
            {
                summary.EmailSmtpPassword = user.EmailPassword;
                summary.EmailSmtpServer = user.EmailSmtpServer;
                summary.EmailSmtpPort = user.EmailSmtpPort ?? 993;
            }
            else
            {
                summary.ErrorMessage = "Email smpt server, smtp app password and port are required";
            }

            return summary;
        }

    }
}
