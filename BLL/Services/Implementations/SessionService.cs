using BLL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;

namespace BLL.Services.Implementations
{
    public class SessionService
    {
        private readonly UserManager _userManager;
        private readonly MediaService _mediaService;

        public SessionService(UserManager userManager, MediaService mediaService)
        {
            _userManager = userManager;
            _mediaService = mediaService;
        }

        //public async Task<UserDTO> GetCurrentUserFromSession(HttpContext httpContext)
        //{
        //    var userIdString = httpContext.Session.GetString("ActiveLoginedUserId");
        //    if (!Guid.TryParse(userIdString, out Guid userId))
        //    {
        //        throw new Exception("User is not authorized!");
        //    }

        //    var user = await _userManager.GetUserById(userId);
        //    if (user == null)
        //    {
        //        throw new Exception("User is not found!");
        //    }

        //    user.HasAvatar = _mediaService.HasAvatar(user.Id);

        //    var userDTO = new UserDTO(user);

        //    return userDTO;
        //}
    }
}
