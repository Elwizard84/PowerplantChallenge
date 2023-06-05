
using Powerplant.API.Config;
using Serilog;

namespace Powerplant.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure Serilog
            LoggerConfig.ConfigureLogger(builder);

            builder.Services.AddControllers();

            // Configure services
            ServiceManager.ConfigureServices(builder.Services);

            builder.Logging.AddSerilog();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}