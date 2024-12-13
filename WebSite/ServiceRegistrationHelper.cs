using Microsoft.Extensions.DependencyInjection;

namespace WebSite
{
    public static class ServiceRegistrationHelper
    {
        /// <summary>
        /// Регистрирует только сервисы уровня бизнес-логики (BLL).
        /// </summary>
        public static void RegisterBusinessLogicServices(IServiceCollection services)
        {
            // Регистрация BLL-сервисов
            services.AddScoped<WebLoginBLL.Services.RoleService>();
            services.AddScoped<WebLoginBLL.Services.UserService>();

            // Пример: добавьте другие сервисы бизнес-логики, если нужно
            // services.AddScoped<SomeOtherBLLService>();
        }
    }
}
