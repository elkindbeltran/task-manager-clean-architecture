namespace TaskManagerCleanArchitecture.Application.Features.Tasks.Commands;

public class ChangeTaskStatusCommandHandler
    : IRequestHandler<ChangeTaskStatusCommand, TaskDto>
{
    private readonly ITaskRepository _repository;
    private readonly IMapper _mapper;

    public ChangeTaskStatusCommandHandler(ITaskRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TaskDto> Handle(ChangeTaskStatusCommand request, CancellationToken cancellationToken)
    {
        var task = await _repository.GetByIdAsync(request.TaskId);

        if (task is null)
            throw new NotFoundException(nameof(TaskItem), request.TaskId);

        task.ChangeStatus(request.Status);

        await _repository.UpdateAsync(task);

        return _mapper.Map<TaskDto>(task);
    }
}