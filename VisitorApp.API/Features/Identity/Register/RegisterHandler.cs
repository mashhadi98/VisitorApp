using VisitorApp.Application.Features.Identity.Register;

namespace VisitorApp.API.Features.Identity.Register;

public class RegisterHandler : PostEndpoint<RegisterRequest, RegisterCommandRequest, RegisterCommandResponse>
{
    public override string? RolesAccess => "";
    
    public RegisterHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
} 