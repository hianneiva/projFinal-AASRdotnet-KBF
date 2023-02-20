using KnowledgeBaseForum.Commons.JWT.Model;
using KnowledgeBaseForum.Commons.JWT;
using KnowledgeBaseForum.Commons.JWT.Impl;
using KnowledgeBaseForum.AdminWebApp.Models.Config;

namespace KnowledgeBaseForum.AdminWebApp.Services
{
    /// <summary>
    /// Implements logic to better organize dependencies between configurations and services reliant on those.
    /// </summary>
    public static class ServiceConfiguration
    {
        /// <summary>
        /// Manages configuration initialization.
        /// </summary>
        /// <param name="services">The app services.</param>
        /// <param name="config">The app configurations.</param>
        /// <returns>The services with configuration setup added.</returns>
        public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<JwtOptions>(config.GetSection(JwtOptions.RootName));
            services.Configure<ApiOptions>(config.GetSection(ApiOptions.RootName));

            return services;
        }

        /// <summary>
        /// Manages scoped dependency injection.
        /// </summary>
        /// <param name="services">The app services.</param>
        /// <returns>The services with scoped dependencies added.</returns>
        public static IServiceCollection AddScopedDepedencies(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
