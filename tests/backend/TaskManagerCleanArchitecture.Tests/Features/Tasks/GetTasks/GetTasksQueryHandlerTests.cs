namespace TaskManagerCleanArchitecture.UnitTests.Features.Tasks.GetTasks;

public class GetTasksQueryHandlerTests
{
    private readonly Mock<ITaskRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetTasksQueryHandler _handler;

    public GetTasksQueryHandlerTests()
    {
        _repositoryMock = new Mock<ITaskRepository>();
        _mapperMock = new Mock<IMapper>();

        _handler = new GetTasksQueryHandler(
            _repositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_Should_Return_Filtered_Tasks()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var query = new GetTasksQuery(
            userId,
            TaskItemStatus.InProgress,
            "desc"
        );

        var tasks = new List<TaskItem>
        {
            new TaskItem("Task 1", userId, string.Empty),
            new TaskItem("Task 2", userId, string.Empty)
        };

        tasks[0].ChangeStatus(TaskItemStatus.InProgress);
        tasks[1].ChangeStatus(TaskItemStatus.InProgress);

        _repositoryMock
            .Setup(r => r.GetFilteredAsync(
                userId,
                TaskItemStatus.InProgress,
                true
            ))
            .ReturnsAsync(tasks);

        _mapperMock
            .Setup(m => m.Map<IEnumerable<TaskDto>>(It.IsAny<IEnumerable<TaskItem>>()))
            .Returns((IEnumerable<TaskItem> source) =>
                source.Select(x => new TaskDto
                {
                    Title = x.Title,
                    UserId = x.UserId,
                    Status = x.Status.ToString()
                }));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        _repositoryMock.Verify(r =>
            r.GetFilteredAsync(userId, TaskItemStatus.InProgress, true),
            Times.Once);
    }
}