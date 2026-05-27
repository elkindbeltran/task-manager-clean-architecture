namespace TaskManagerCleanArchitecture.Application.Features.Tasks.Commands;

public class CreateTaskCommandHandler
    : IRequestHandler<CreateTaskCommand, TaskDto>
{
    private readonly ITaskRepository _repository;
    private readonly IMapper _mapper;

    public CreateTaskCommandHandler(ITaskRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = new TaskItem(request.Title, request.UserId, request.AdditionalData);

        await _repository.AddAsync(task);

        return _mapper.Map<TaskDto>(task);
    }
}