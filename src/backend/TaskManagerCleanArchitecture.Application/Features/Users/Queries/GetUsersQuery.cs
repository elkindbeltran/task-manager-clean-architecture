namespace TaskManagerCleanArchitecture.Application.Features.Users.Queries;

public record GetUsersQuery() : IRequest<IEnumerable<UserDto>>;