namespace TaskManagerCleanArchitecture.Application.Features.Tasks.Commands;

public record ChangeTaskStatusCommand(Guid TaskId, TaskItemStatus Status) : IRequest<TaskDto>;