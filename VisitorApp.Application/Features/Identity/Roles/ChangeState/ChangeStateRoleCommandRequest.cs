using VisitorApp.Application.Common.Messaging;

namespace VisitorApp.Application.Features.Identity.Roles.ChangeState;

public class ChangeStateRoleCommandRequest : IRequestBase<ChangeStateRoleCommandResponse>
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
} 