namespace TaskManagerCleanArchitecture.Application.Mappings;

[ExcludeFromCodeCoverage]
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();

        CreateMap<CreateUserCommand, User>()
            .ConstructUsing(src => new User(src.Name, src.Email));
    }
}
