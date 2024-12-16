using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace WebAuthCoreBLL.Helpers
{
  public static class AuthHelper
  {
    /// <summary>
    /// Configures authentication and authorization based on provided settings.
    /// </summary>
    /// <param name="services">The service collection to add authentication services to.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <param name="enableJwt">Enable JWT authentication.</param>
    /// <param name="enableCookies">Enable Cookie authentication.</param>
    public static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration, bool enableJwt, bool enableCookies)
    {
      // Configure JWT authentication if enabled
      if (enableJwt)
      {
        JwtAuthHelper.ConfigureJwt(services, configuration);
      }

      // Configure Cookie authentication if enabled
      if (enableCookies)
      {
        CookieAuthHelper.ConfigureCookies(services);
      }

      // Add authorization policies based on enabled authentication methods
      services.AddAuthorization(options =>
      {
        if (enableJwt)
        {
          options.AddPolicy("JwtPolicy", policy =>
          {
            policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            policy.RequireAuthenticatedUser();
          });
        }
        else
        {
          // Добавляем политику, которая всегда разрешает доступ
          options.AddPolicy("JwtPolicy", policy =>
          {
            policy.RequireAssertion(_ => true); // Всегда разрешать доступ
          });
        }

        if (enableCookies)
        {
          options.AddPolicy("CookiePolicy", policy =>
          {
            policy.AuthenticationSchemes.Add(CookieAuthenticationDefaults.AuthenticationScheme);
            policy.RequireAuthenticatedUser();
          });
        }
        else
        {
          // Добавляем политику, которая всегда разрешает доступ
          options.AddPolicy("CookiePolicy", policy =>
          {
            policy.RequireAssertion(_ => true); // Всегда разрешать доступ
          });
        }
      });
    }
  }
}
