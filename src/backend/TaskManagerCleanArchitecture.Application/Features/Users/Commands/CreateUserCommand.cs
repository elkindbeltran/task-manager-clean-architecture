namespace TaskManagerCleanArchitecture.Application.Features.Users.Commands;

public record CreateUserCommand(string Name, string Email) : IRequest<UserDto>;