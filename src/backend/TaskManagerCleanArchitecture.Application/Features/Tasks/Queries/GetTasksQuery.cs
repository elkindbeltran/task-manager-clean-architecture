namespace TaskManagerCleanArchitecture.Application.Features.Tasks.Queries;

public record GetTasksQuery(Guid? UserId, TaskItemStatus? Status, string? Order) : IRequest<IEnumerable<TaskDto>>;