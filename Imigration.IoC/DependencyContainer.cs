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

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<IQuestionService, QuestionService>();

            #endregion

            #region Repositories

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISiteSettingRepository, SiteSettingRepository>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();

            #endregion
        }
    }
}
