namespace TaskManagerCleanArchitecture.Application.Features.Tasks.Queries;

public class GetTasksQueryHandler
    : IRequestHandler<GetTasksQuery, IEnumerable<TaskDto>>
{
    private readonly ITaskRepository _repository;
    private readonly IMapper _mapper;

    public GetTasksQueryHandler(ITaskRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TaskDto>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _repository.GetFilteredAsync(request.UserId, request.Status, request.Order == "desc");

        return _mapper.Map<IEnumerable<TaskDto>>(tasks);
    }
}