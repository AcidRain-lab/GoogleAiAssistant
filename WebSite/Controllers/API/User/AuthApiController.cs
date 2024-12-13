using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebLoginBLL.DTO;
using WebLoginBLL.Services;

namespace WebSite.Controllers.API.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthApiController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly UserService _userService;

        public AuthApiController(AuthService authService, UserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        /// <summary>
        /// API метод для входа с использованием JWT.
        /// </summary>
        /// <param name="model">Данные для входа.</param>
        /// <returns>JWT-токен при успешной аутентификации.</returns>
        [HttpPost("loginApi")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginApi([FromBody] LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input.");
            }

            var user = await _userService.ValidateUserAsync(model.UserName, model.Password);
            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = _authService.GenerateJwtToken(user);
            return Ok(new { token });
        }

        /// <summary>
        /// API метод для выхода.
        /// </summary>
        /// <returns>Сообщение об успешном выходе.</returns>
        [HttpPost("logoutApi")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult LogoutApi()
        {
            // Выход для API (дополнительная логика может быть добавлена).
            return Ok(new { message = "Logged out successfully." });
        }

        /// <summary>
        /// Пример защищенного API метода.
        /// </summary>
        /// <returns>Пример доступа к данным для аутентифицированных пользователей.</returns>
        [HttpGet("protected")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult Protected()
        {
            return Ok(new { message = "This is a protected endpoint.", user = User.Identity?.Name });
        }
    }
}
