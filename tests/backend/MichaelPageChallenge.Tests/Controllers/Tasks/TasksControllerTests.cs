namespace MichaelPageChallenge.UnitTests.Controllers.Tasks;

public class TasksControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILogger<TasksController>> _loggerMock;
    private readonly TasksController _controller;

    public TasksControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<TasksController>>();

        _controller = new TasksController(
            _mediatorMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Get_ShouldReturnOk_WithTasks()
    {
        // Arrange
        var tasks = new List<TaskDto>
            {
                new TaskDto
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Task"
                }
            };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetTasksQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(tasks);

        // Act
        var result = await _controller.Get(null, null, "asc");

        // Assert
        var okResult = result.Result as OkObjectResult;

        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(tasks);

        _mediatorMock.Verify(
            m => m.Send(It.Is<GetTasksQuery>(q =>
                q.Order == "asc"
            ), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public async Task Create_ShouldReturnCreated_WithTask()
    {
        // Arrange
        var command = new CreateTaskCommand(
            "Test Task",
            Guid.NewGuid(),
            "Description"           
        );

        var createdTask = new TaskDto
        {
            Id = Guid.NewGuid(),
            Title = command.Title
        };

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdTask);

        // Act
        var result = await _controller.Create(command);

        // Assert
        var createdResult = result.Result as CreatedResult;

        createdResult.Should().NotBeNull();
        createdResult!.StatusCode.Should().Be(201);
        createdResult.Value.Should().BeEquivalentTo(createdTask);

        _mediatorMock.Verify(
            m => m.Send(command, It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public async Task ChangeStatus_ShouldReturnBadRequest_WhenIdsDoNotMatch()
    {
        // Arrange
        var routeId = Guid.NewGuid();
        var command = new ChangeTaskStatusCommand(
            Guid.NewGuid(),
            TaskItemStatus.Done
        );

        // Act
        var result = await _controller.ChangeStatus(routeId, command);

        // Assert
        var badRequest = result.Result as BadRequestObjectResult;

        badRequest.Should().NotBeNull();
        badRequest!.StatusCode.Should().Be(400);
        badRequest.Value.Should().Be("Task ID mismatch");

        _mediatorMock.Verify(
            m => m.Send(It.IsAny<ChangeTaskStatusCommand>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    [Fact]
    public async Task ChangeStatus_ShouldReturnOk_WhenIdsMatch()
    {
        // Arrange
        var taskId = Guid.NewGuid();

        var command = new ChangeTaskStatusCommand(
            taskId,
            TaskItemStatus.Done
        );

        var updatedTask = new TaskDto
        {
            Id = taskId,
            Status = TaskItemStatus.Done.ToString()
        };

        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedTask);

        // Act
        var result = await _controller.ChangeStatus(taskId, command);

        // Assert
        var okResult = result.Result as OkObjectResult;

        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(updatedTask);

        _mediatorMock.Verify(
            m => m.Send(command, It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}