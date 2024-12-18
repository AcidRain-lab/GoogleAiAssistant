using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebAuthCoreBLL.Helpers
{
    public static class JwtAuthHelper
    {
        public static void ConfigureJwt(IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            string secretKey = jwtSettings["SecretKey"];
            string issuer = jwtSettings["Issuer"];
            string audience = jwtSettings["Audience"];

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                    };
                });
        }

        public static string GetRoleFromToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            return roleClaim?.Value; // Возвращает роль, если она есть
        }
    }
}



//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using Microsoft.Extensions.Configuration;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;

//namespace WebAuthCoreBLL.Helpers
//{
//  public static class JwtAuthHelper
//  {
//    public static void ConfigureJwt(IServiceCollection services, IConfiguration configuration)
//    {
//      var jwtSettings = configuration.GetSection("JwtSettings");
//      string secretKey = jwtSettings["SecretKey"];
//      string issuer = jwtSettings["Issuer"];
//      string audience = jwtSettings["Audience"];

//      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//          .AddJwtBearer(options =>
//          {
//            options.TokenValidationParameters = new TokenValidationParameters
//            {
//              ValidateIssuer = true,
//              ValidateAudience = true,
//              ValidateLifetime = true,
//              ValidateIssuerSigningKey = true,
//              ValidIssuer = issuer,
//              ValidAudience = audience,
//              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
//            };
//          });



//    }

//        public static string GetRoleFromToken(string token)
//        {
//            if (string.IsNullOrEmpty(token))
//            {
//                throw new ArgumentNullException(nameof(token));
//            }

//            var handler = new JwtSecurityTokenHandler();
//            var jwtToken = handler.ReadJwtToken(token);

//            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
//            return roleClaim?.Value; // Возвращает роль, если она есть
//        }
//    }
//}
