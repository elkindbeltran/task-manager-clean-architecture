namespace TaskManagerCleanArchitecture.IntegrationTests.Controllers.Users;

public class UsersControllerTests : TestBase
{
    public UsersControllerTests(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Get_ShouldReturnOk()
    {
        // Act
        var response = await Client.GetAsync("/api/users");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var users = await response.Content
            .ReadFromJsonAsync<IEnumerable<UserDto>>();

        users.Should().NotBeNull();
    }

    [Fact]
    public async Task Create_ShouldCreateUser()
    {
        // Arrange
        var command = new CreateUserCommand(
            "test",
            "test@gmail.com"
        );

        // Act
        var response = await Client.PostAsJsonAsync("/api/users", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var user = await response.Content
            .ReadFromJsonAsync<UserDto>();

        user.Should().NotBeNull();

        user!.Name.Should().Be("test");
        user.Email.Should().Be("test@gmail.com");
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenEmailIsInvalid()
    {
        // Arrange
        var command = new CreateUserCommand(
            "test",
            "Invalid-Email"
        );

        // Act
        var response = await Client.PostAsJsonAsync("/api/users", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}