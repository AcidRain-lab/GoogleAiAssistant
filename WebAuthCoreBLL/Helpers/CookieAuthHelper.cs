using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebAuthCoreBLL.Helpers
{
  public static class CookieAuthHelper
  {
    public static void ConfigureCookies(IServiceCollection services)
    {
      services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
          .AddCookie(options =>
          {
            options.LoginPath = "/Auth/LoginBasic";
            options.AccessDeniedPath = "/Auth/AccessDenied";

            // Обработка перенаправления для API
            options.Events.OnRedirectToLogin = context =>
            {
              if (context.Request.Path.StartsWithSegments("/api"))
              {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
              }

              context.Response.Redirect(context.RedirectUri);
              return Task.CompletedTask;
            };
          });
    }
  }
}




