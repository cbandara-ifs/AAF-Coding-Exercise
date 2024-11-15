using LegacyApp.Application.Contracts;
using LegacyApp.Application.Contracts.Persistence;
using LegacyApp.Application.Services;
using LegacyApp.DataAccessLayer;
using LegacyApp.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace LegacyApp
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddSingleton<IUserDataAccessService, UserDataAccessService>();

            return services;
        }
    }
}
