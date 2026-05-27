namespace TaskManagerCleanArchitecture.UnitTests.Controllers.Users;

public class UsersControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILogger<UsersController>> _loggerMock;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<UsersController>>();

        _controller = new UsersController(
            _mediatorMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Get_ShouldReturnOk_WithUsers()
    {
        // Arrange
        var users = new List<UserDto>
            {
                new UserDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Test User"
                }
            };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetUsersQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        // Act
        var result = await _controller.Get();

        // Assert
        var okResult = result.Result as OkObjectResult;

        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(users);

        _mediatorMock.Verify(
            m => m.Send(It.IsAny<GetUsersQuery>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public async Task Create_ShouldReturnCreated_WithUser()
    {
        // Arrange
        var command = new CreateUserCommand(
            "New User",
            "test@gmail.com"
        );

        var createdUser = new UserDto
        {
            Id = Guid.NewGuid(),
            Name = command.Name
        };

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdUser);

        // Act
        var result = await _controller.Create(command);

        // Assert
        var createdResult = result.Result as CreatedResult;

        createdResult.Should().NotBeNull();
        createdResult!.StatusCode.Should().Be(201);
        createdResult.Value.Should().BeEquivalentTo(createdUser);

        _mediatorMock.Verify(
            m => m.Send(command, It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}