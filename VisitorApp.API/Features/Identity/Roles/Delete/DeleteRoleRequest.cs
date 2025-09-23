namespace VisitorApp.API.Features.Identity.Roles.Delete;

public class DeleteRoleRequest : RequestBase
{
    public DeleteRoleRequest() : base("Roles/{Id}") { }

    public Guid Id { get; set; }
} 