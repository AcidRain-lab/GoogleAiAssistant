using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;

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
  }
}
