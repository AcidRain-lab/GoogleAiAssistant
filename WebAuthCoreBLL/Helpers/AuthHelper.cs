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

            // Configure authorization policies
            ConfigureAuthorization(services, enableJwt, enableCookies);
        }


        //private static void ConfigureAuthorization(IServiceCollection services, bool enableJwt, bool enableCookies)
        //{
        //    services.AddAuthorization(options =>
        //    {
        //        // Политика для всех авторизованных
        //        options.AddPolicy("Authenticated", policy =>
        //        {
        //            policy.RequireAuthenticatedUser();
        //        });

        //        // Политика для администраторов
        //        options.AddPolicy("AdminOnly", policy =>
        //        {
        //            policy.RequireAssertion(context =>
        //            {
        //                var token = context.User.FindFirst("Authorization")?.Value; // Найти токен
        //                if (string.IsNullOrEmpty(token)) return false;

        //                var role = JwtAuthHelper.GetRoleFromToken(token);
        //                return role == "Admin";
        //            });
        //        });

        //        // Политика для администраторов и пользователей
        //        options.AddPolicy("AdminOrUser", policy =>
        //        {
        //            policy.RequireAssertion(context =>
        //            {
        //                var token = context.User.FindFirst("Authorization")?.Value; // Найти токен
        //                if (string.IsNullOrEmpty(token)) return false;

        //                var role = JwtAuthHelper.GetRoleFromToken(token);
        //                return role == "Admin" || role == "User";
        //            });
        //        });

        //        if (enableJwt)
        //        {
        //            options.AddPolicy("JwtPolicy", policy =>
        //            {
        //                policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        //                policy.RequireAuthenticatedUser();
        //            });
        //        }

        //        if (enableCookies)
        //        {
        //            options.AddPolicy("CookiePolicy", policy =>
        //            {
        //                policy.AuthenticationSchemes.Add(CookieAuthenticationDefaults.AuthenticationScheme);
        //                policy.RequireAuthenticatedUser();
        //            });
        //        }
        //    });
        //}


        /// <summary>
        /// Configures authorization policies for JWT, Cookies, and role-based access.
        /// </summary>
        private static void ConfigureAuthorization(IServiceCollection services, bool enableJwt, bool enableCookies)
        {
            services.AddAuthorization(options =>
            {
                // Политика для всех авторизованных пользователей
                options.AddPolicy("Authenticated", policy =>
                {
                    policy.RequireAuthenticatedUser();
                });

                // Политика только для администраторов
                options.AddPolicy("AdminOnly", policy =>
                {
                    policy.RequireRole("Admin");
                });

                // Политика для администраторов и пользователей
                options.AddPolicy("AdminOrUser", policy =>
                {
                    policy.RequireRole("Admin", "User");
                });

                // Политика для JWT
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
                    options.AddPolicy("JwtPolicy", policy =>
                    {
                        policy.RequireAssertion(_ => true); // Разрешить доступ, если JWT отключен
                    });
                }

                // Политика для Cookies
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
                    options.AddPolicy("CookiePolicy", policy =>
                    {
                        policy.RequireAssertion(_ => true); // Разрешить доступ, если Cookies отключены
                    });
                }
            });
        }
    }
}
