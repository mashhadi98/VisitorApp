namespace VisitorApp.API.Features.Identity.Users.ChangeState;

public class ChangeStateUserRequest : RequestBase
{
    public ChangeStateUserRequest() : base("Users/{Id}/ChangeState") { }

    public Guid Id { get; set; }
    public bool IsActive { get; set; }
} 