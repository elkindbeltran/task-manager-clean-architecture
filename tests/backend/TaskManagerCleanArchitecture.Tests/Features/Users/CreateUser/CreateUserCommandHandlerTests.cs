namespace TaskManagerCleanArchitecture.UnitTests.Features.Users.CreateUser;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _repositoryMock = new Mock<IUserRepository>();
        _mapperMock = new Mock<IMapper>();

        _handler = new CreateUserCommandHandler(
            _repositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_Should_Create_User_And_Return_Dto()
    {
        // Arrange
        var command = new CreateUserCommand(
            "John Doe",
            "john@test.com"
        );

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        _mapperMock
            .Setup(m => m.Map<UserDto>(It.IsAny<User>()))
            .Returns((User u) => new UserDto
            {
                Name = u.Name,
                Email = u.Email
            });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("John Doe");
        result.Email.Should().Be("john@test.com");

        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);

        _mapperMock.Verify(
            m => m.Map<UserDto>(It.IsAny<User>()),
            Times.Once);
    }
}