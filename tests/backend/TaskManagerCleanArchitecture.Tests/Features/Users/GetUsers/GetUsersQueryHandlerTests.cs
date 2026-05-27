namespace TaskManagerCleanArchitecture.UnitTests.Features.Users.GetUsers;

public class GetUsersQueryHandlerTests
{
    private readonly Mock<IUserRepository> _repositoryMock;
    private readonly IMapper _mapper;
    private readonly GetUsersQueryHandler _handler;

    public GetUsersQueryHandlerTests()
    {
        _repositoryMock = new Mock<IUserRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UserProfile>();
        });

        _mapper = config.CreateMapper();

        _handler = new GetUsersQueryHandler(_repositoryMock.Object, _mapper);
    }

    [Fact]
    public async Task Should_Return_Users_List()
    {
        // Arrange
        var users = new List<User>
        {
            new("John", "john@test.com"),
            new("Jane", "jane@test.com")
        };

        _repositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(users);

        // Act
        var result = await _handler.Handle(new GetUsersQuery(), CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
    }
}