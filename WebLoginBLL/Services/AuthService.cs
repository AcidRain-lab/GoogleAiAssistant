using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAuthCoreBLL.Helpers;
using WebLoginBLL.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace WebLoginBLL.Services
{
    public class AuthService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IOptions<JwtSettings> jwtSettings, IHttpContextAccessor httpContextAccessor)
        {
            _jwtSettings = jwtSettings.Value ?? throw new ArgumentNullException(nameof(jwtSettings));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        /// <summary>
        /// Генерация JWT-токена для аутентификации.
        /// </summary>
        /// <param name="user">Данные пользователя.</param>
        /// <returns>JWT-токен.</returns>
        public string GenerateJwtToken(UserDTO user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.RoleName ?? "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Настройка аутентификации через куки.
        /// </summary>
        /// <param name="user">Данные пользователя.</param>
        public async Task SetCookieAuthentication(UserDTO user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            // Настраиваем Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.RoleName ?? "User")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Устанавливаем куки с Claims
            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity)
            );

            // Устанавливаем данные в сессии
            var session = _httpContextAccessor.HttpContext.Session;
            session.SetString("ActiveLoginedUserId", user.Id.ToString());
            session.SetString("ActiveLoginedUser", user.UserName);
            session.SetString("UserRole", user.RoleName ?? "User");
        }

        /// <summary>
        /// Выход из системы и удаление сессии.
        /// </summary>
        public async Task Logout()
        {
            if (_httpContextAccessor.HttpContext == null)
                throw new InvalidOperationException("HTTP context is not available.");

            // Удаляем данные из сессии
            var session = _httpContextAccessor.HttpContext.Session;
            session.Remove("ActiveLoginedUserId");
            session.Remove("ActiveLoginedUser");
            session.Remove("UserRole");

            // Удаляем куки
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Полностью очищаем сессию
            session.Clear();
        }

        /// <summary>
        /// Проверка пароля.
        /// </summary>
        /// <param name="password">Введенный пароль.</param>
        /// <param name="storedHash">Хранимый хеш пароля.</param>
        /// <returns>Результат проверки.</returns>
        public bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash))
                throw new ArgumentException("Password or stored hash cannot be null or empty.");

            return SecurePasswordHasher.Verify(password, storedHash);
        }
    }
}
