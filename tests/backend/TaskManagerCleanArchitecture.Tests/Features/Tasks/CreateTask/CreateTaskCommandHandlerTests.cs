namespace TaskManagerCleanArchitecture.UnitTests.Features.Tasks.CreateTask;

public class CreateTaskCommandHandlerTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateTaskCommandHandler _handler;

    public CreateTaskCommandHandlerTests()
    {
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _mapperMock = new Mock<IMapper>();

        _handler = new CreateTaskCommandHandler(
            _taskRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_Should_Create_Task_Successfully()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var command = new CreateTaskCommand(
            "Test Task",
            userId,
            "{\"priority\":\"High\"}"
        );

        var user = new User("John", "john@test.com");

        _taskRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<TaskItem>()))
            .Returns(Task.CompletedTask);

        _mapperMock
            .Setup(m => m.Map<TaskDto>(It.IsAny<TaskItem>()))
            .Returns((TaskItem t) => new TaskDto
            {
                Title = t.Title,
                UserId = t.UserId,
                Status = t.Status.ToString()
            });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("Test Task");
        result.UserId.Should().Be(userId);

        _taskRepositoryMock.Verify(x => x.AddAsync(
            It.Is<TaskItem>(t =>
                t.Title == "Test Task" &&
                t.UserId == userId)
        ), Times.Once);
    }
}