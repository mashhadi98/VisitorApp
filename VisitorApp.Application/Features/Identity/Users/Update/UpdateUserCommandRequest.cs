using VisitorApp.Application.Common.Messaging;

namespace VisitorApp.Application.Features.Identity.Users.Update;

public class UpdateUserCommandRequest : IRequestBase<UpdateUserCommandResponse>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool? IsActive { get; set; }
} 