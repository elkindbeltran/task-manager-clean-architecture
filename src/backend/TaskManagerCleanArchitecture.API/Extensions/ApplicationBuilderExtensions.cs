namespace TaskManagerCleanArchitecture.API.Extensions;

[ExcludeFromCodeCoverage]
public static class ApplicationBuilderExtensions
{
    public static WebApplication UseApiMiddlewares(this WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<CorrelationIdMiddleware>();
        app.UseMiddleware<ExceptionMiddleware>();        

        app.MapControllers();

        app.UseCors("AllowAngular");

        return app;
    }
}