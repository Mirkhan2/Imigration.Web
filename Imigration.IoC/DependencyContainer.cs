using Imigration.Application.Services.Implementions;
using Imigration.Application.Services.Interfaces;
using Imigration.DataLayer.Repositories;
using Imigration.Domains.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Imigration.IoC
{
    public class DependencyContainer
    {
        public static void RegisterDependencies(IServiceCollection services)
        {
            #region Services
            services.AddScoped<IUserService , UserService>();

            #endregion

            #region Repositories
            services.AddScoped<IUserRepository , UserRepository>();
            #endregion
        }
    }
}
