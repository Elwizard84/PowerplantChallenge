using Powerplant.Domain.Interfaces;
using Powerplant.Infrastructure.Services;

namespace Powerplant.API.Config
{
    public static class ServiceManager
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddAutoMapper(typeof(Program));

            // Add services
            services.AddTransient<IProductionPlanService, ProductionPlanService>();
        }
    }
}
