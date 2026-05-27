namespace TaskManagerCleanArchitecture.Application.Features.Tasks.Commands;

public record CreateTaskCommand(string Title, Guid UserId, string AdditionalData) : IRequest<TaskDto>;