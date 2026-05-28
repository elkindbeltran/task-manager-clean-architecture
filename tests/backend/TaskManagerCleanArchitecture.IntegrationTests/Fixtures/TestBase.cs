using TaskManagerCleanArchitecture.IntegrationTests.Infrastructure;

namespace TaskManagerCleanArchitecture.IntegrationTests.Fixtures;

public abstract class TestBase
    : IClassFixture<CustomWebApplicationFactory>
{
    protected readonly HttpClient Client;

    protected TestBase(CustomWebApplicationFactory factory)
    {
        Client = factory.CreateClient();
    }
}