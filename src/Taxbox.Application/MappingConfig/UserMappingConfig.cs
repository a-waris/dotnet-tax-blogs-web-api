using Taxbox.Domain.Auth;
using Taxbox.Domain.Entities;
using Mapster;
using Taxbox.Application.Features.Users;
using Taxbox.Application.Features.Users.CreateUser;

namespace Taxbox.Application.MappingConfig;

public class UserMappingConfig : IMappingConfig
{
    public void ApplyConfig()
    {
        TypeAdapterConfig<CreateUserRequest, User>
            .ForType()
            .Map(dest => dest.Role,
                opt => opt.IsAdmin ? Roles.Admin : Roles.User);
        
        TypeAdapterConfig<User, GetUserResponse>
            .ForType()
            .Map(dest => dest.IsAdmin,
                x => x.Role == Roles.Admin);
    }
}