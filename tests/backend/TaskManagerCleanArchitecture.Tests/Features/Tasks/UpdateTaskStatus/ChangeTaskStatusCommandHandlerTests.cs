namespace TaskManagerCleanArchitecture.UnitTests.Features.Tasks.UpdateTaskStatus;

public class ChangeTaskStatusCommandHandlerTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ChangeTaskStatusCommandHandler _handler;

    public ChangeTaskStatusCommandHandlerTests()
    {
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _mapperMock = new Mock<IMapper>();

        _handler = new ChangeTaskStatusCommandHandler(
            _taskRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_Should_Change_Status_Successfully()
    {
        // Arrange
        var task = new TaskItem("Task", Guid.NewGuid(), string.Empty);

        _taskRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(task);

        _taskRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<TaskItem>()))
            .Returns(Task.CompletedTask);

        var command = new ChangeTaskStatusCommand(
            task.Id,
            TaskItemStatus.InProgress
        );

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        task.Status.Should().Be(TaskItemStatus.InProgress);

        _taskRepositoryMock.Verify(x => x.UpdateAsync(task), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Task_Not_Found()
    {
        // Arrange
        _taskRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((TaskItem?)null);

        var command = new ChangeTaskStatusCommand(
            Guid.NewGuid(),
            TaskItemStatus.InProgress
        );

        // Act
        Func<Task> act = async () =>
            await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Invalid_Status_Transition()
    {
        // Arrange
        var task = new TaskItem("Task", Guid.NewGuid(), string.Empty);

        _taskRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(task);

        var command = new ChangeTaskStatusCommand(
            task.Id,
            TaskItemStatus.Done
        );

        // Act
        Func<Task> act = async () =>
            await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}