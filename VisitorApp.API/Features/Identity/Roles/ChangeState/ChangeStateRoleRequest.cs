namespace VisitorApp.API.Features.Identity.Roles.ChangeState;

public class ChangeStateRoleRequest : RequestBase
{
    public ChangeStateRoleRequest() : base("Roles/{Id}/ChangeState") { }

    public Guid Id { get; set; }
    public bool IsActive { get; set; }
} 