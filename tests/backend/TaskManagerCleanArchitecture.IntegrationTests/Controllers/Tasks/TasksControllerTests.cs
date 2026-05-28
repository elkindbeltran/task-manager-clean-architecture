namespace TaskManagerCleanArchitecture.IntegrationTests.Controllers.Tasks;

public class TasksControllerTests : TestBase
{
    public TasksControllerTests(CustomWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Get_ShouldReturnOk()
    {
        // Act
        var response = await Client.GetAsync("/api/tasks");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tasks = await response.Content
            .ReadFromJsonAsync<IEnumerable<TaskDto>>();

        tasks.Should().NotBeNull();
    }

    [Fact]
    public async Task Create_ShouldCreateTask()
    {
        // Arrange
        var command = new CreateTaskCommand(
            "Integration Task",
            Guid.NewGuid(),
            "{}"            
        );

        // Act
        var response = await Client.PostAsJsonAsync("/api/tasks", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var task = await response.Content
            .ReadFromJsonAsync<TaskDto>();

        task.Should().NotBeNull();

        task!.Title.Should().Be("Integration Task");

        task.AdditionalData.Should()
            .Be("{}");
    }

    [Fact]
    public async Task Get_ShouldFilterByStatus()
    {
        // Act
        var response = await Client.GetAsync("/api/tasks?status=Pending");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var tasks = await response.Content
            .ReadFromJsonAsync<IEnumerable<TaskDto>>();

        tasks.Should().NotBeNull();
    }

    [Fact]
    public async Task ChangeStatus_ShouldReturnBadRequest_WhenIdsMismatch()
    {
        // Arrange
        var routeId = Guid.NewGuid();

        var command = new ChangeTaskStatusCommand(
            Guid.NewGuid(),
            TaskItemStatus.Done
        );

        // Act
        var response = await Client.PutAsJsonAsync($"/api/tasks/{routeId}/status", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenTitleIsEmpty()
    {
        // Arrange
        var command = new CreateTaskCommand(
            "",
            Guid.NewGuid(),
            "{}"
        );

        // Act
        var response = await Client.PostAsJsonAsync("/api/tasks", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ChangeStatus_ShouldUpdateTaskStatus()
    {
        // Arrange
        var createCommand = new CreateTaskCommand(
            "Integration Task",
            Guid.NewGuid(),
            "{}"
        );

        var createResponse = await Client.PostAsJsonAsync(
            "/api/tasks",
            createCommand);

        var createdTask = await createResponse.Content
            .ReadFromJsonAsync<TaskDto>();

        var statusCommand = new ChangeTaskStatusCommand(
            createdTask!.Id,
            TaskItemStatus.InProgress
        );

        // Act
        var response = await Client.PutAsJsonAsync($"/api/tasks/{createdTask.Id}/status", statusCommand);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedTask = await response.Content
            .ReadFromJsonAsync<TaskDto>();

        updatedTask!.Status.Should()
            .Be(TaskItemStatus.InProgress.ToString());
    }
}