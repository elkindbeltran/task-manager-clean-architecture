namespace TaskManagerCleanArchitecture.IntegrationTests.Infrastructure;

public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services
                .SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }

            var contextDescriptor = services
                .SingleOrDefault(d =>
                    d.ServiceType == typeof(AppDbContext));

            if (contextDescriptor != null)
            {
                services.Remove(contextDescriptor);
            }

            services.RemoveAll<IDbContextOptionsConfiguration<AppDbContext>>();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";

                    options.DefaultChallengeScheme = "Test";
                })
                .AddScheme<
                    AuthenticationSchemeOptions,
                    TestAuthHandler>(
                    "Test",
                    options => { });
        });
    }
}