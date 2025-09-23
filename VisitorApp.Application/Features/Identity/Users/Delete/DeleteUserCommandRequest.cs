using VisitorApp.Application.Common.Messaging;

namespace VisitorApp.Application.Features.Identity.Users.Delete;

public class DeleteUserCommandRequest : IRequestBase<DeleteUserCommandResponse>
{
    public Guid Id { get; set; }
} 