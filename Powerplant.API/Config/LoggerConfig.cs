using Serilog;
using Serilog.Formatting.Compact;

public static class LoggerConfig
{
    public static void ConfigureLogger(WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Configuration)
                    .Enrich.FromLogContext()
                    // Other Serilog configuration options can be added here
                    .CreateLogger();
    }
}
