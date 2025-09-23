using VisitorApp.Application.Common.Messaging;

namespace VisitorApp.Application.Features.Identity.Roles.Delete;

public class DeleteRoleCommandRequest : IRequestBase<DeleteRoleCommandResponse>
{
    public Guid Id { get; set; }
} 