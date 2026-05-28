namespace TaskManagerCleanArchitecture.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TasksController> _logger;

    public TasksController(IMediator mediator, ILogger<TasksController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskDto>>> Get([FromQuery] Guid? userId, [FromQuery] TaskItemStatus? status, [FromQuery] string? order = "asc")
    {
        _logger.LogInformation("Fetching tasks with filters");

        var result = await _mediator.Send(new GetTasksQuery(userId, status, order));

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<TaskDto>> Create(CreateTaskCommand command)
    {
        _logger.LogInformation("Creating task");

        var result = await _mediator.Send(command);

        return Created("", result);
    }

    [HttpPut("{id}/status")]
    public async Task<ActionResult<TaskDto>> ChangeStatus(Guid id, ChangeTaskStatusCommand command)
    {
        if (id != command.TaskId)
            return BadRequest("Task ID mismatch");

        _logger.LogInformation("Changing task status for {TaskId}", id);

        var result = await _mediator.Send(command);

        return Ok(result);
    }
}