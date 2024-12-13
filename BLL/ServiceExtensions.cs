using BLL.Services.Implementations;
using BLL.Services;
using DAL.Repositories.Implementations;
using DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{

    public static class ServiceExtensions
    {
        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            services.AddScoped<BLL.UserManager>();
            services.AddScoped<RoleManager>();
            services.AddScoped<ContactsTypeManager>();
            services.AddScoped<ContactManager>();
            services.AddScoped<PhoneManager>();
            services.AddScoped<LeadManager>();
            services.AddScoped<MediaService>();

            return services;
        }

        public static IServiceCollection AddTransactionManagers(this IServiceCollection services)
        {
            services.AddTransient<IContactManager, ContactManager>();
            services.AddTransient<ILeadManager, LeadManager>();
            services.AddTransient<IUserManager, BLL.Services.Implementations.UserManager>();
            services.AddTransient<IAppointmentManager, AppointmentManager>();


            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<ICountryRepository, CountryRepository>();
            services.AddTransient<ILocationRepository, LocationRepository>();
            services.AddTransient<ILeadRepository, LeadRepository>();
            services.AddTransient<IContactRepository, ContactRepository>();
            services.AddTransient<IStateRepository, StateRepository>();
            services.AddTransient<IJobCategoryRepository, JobCategoryRepository>();
            services.AddTransient<IPhoneTypesRepository, PhoneTypesRepository>();
            services.AddTransient<ILeadSourceRepository, LeadSourceRepository>();
            services.AddTransient<IWorkTypeRepository, WorkTypeRepository>();
            services.AddTransient<ITradeTypeRepository, TradeTypeRepository>();
            services.AddTransient<IPhoneRepository, PhoneRepository>();
            services.AddTransient<IMediaDataRepository, MediaDataRepository>();
            services.AddTransient<IAvatarRepository, AvatarRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IContactTypeRepository, ContactTypeRepository>();
            services.AddTransient<IContactTypeListRepository, ContactTypeListRepository>();
            services.AddTransient<IAppointmentRepository, AppointmentRepository>();


            return services;
        }
    }

}
