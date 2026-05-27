namespace TaskManagerCleanArchitecture.API.Extensions;

[ExcludeFromCodeCoverage]
public static class SerilogExtensions
{
    public static IHostBuilder AddSerilogConfiguration(this IHostBuilder host, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        host.UseSerilog();

        return host;
    }
}