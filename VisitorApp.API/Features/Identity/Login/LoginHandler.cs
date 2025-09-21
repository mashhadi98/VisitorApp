using VisitorApp.Application.Features.Identity.Login;

namespace VisitorApp.API.Features.Identity.Login;

public class LoginHandler : PostEndpoint<LoginRequest, LoginCommandRequest, LoginCommandResponse>
{
    public override string? RolesAccess => "";
    
    public LoginHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
} 