using KnowledgeBaseForum.DataAccessLayer.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KnowledgeBaseForum.DataAccessLayer.DependencyInjection
{
    public static class DependencyInjectionHelper
    {
        public static IServiceCollection AddInfrastructureDb(this IServiceCollection services, IConfiguration configuration) 
        {
            ServiceProvider provider = services.AddDbContext<KbfContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
                                               .BuildServiceProvider();
            KbfContext context = provider.GetRequiredService<KbfContext>();
            context.Database.EnsureCreated();

            return services;
        }
    }
}
