using VisitorApp.Application.Common.Messaging;

namespace VisitorApp.Application.Features.Identity.Users.ChangeState;

public class ChangeStateUserCommandRequest : IRequestBase<ChangeStateUserCommandResponse>
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
} 