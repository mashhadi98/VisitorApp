using VisitorApp.Application.Common.Messaging;

namespace VisitorApp.Application.Features.Identity.Roles.Update;

public class UpdateRoleCommandRequest : IRequestBase<UpdateRoleCommandResponse>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool? IsActive { get; set; }
} 