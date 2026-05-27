var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices();

builder.Host.AddSerilogConfiguration(builder.Configuration);

var app = builder.Build();

app.UseApiMiddlewares(app.Environment);

app.Run();

public partial class Program
{
}