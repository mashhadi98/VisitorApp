namespace VisitorApp.API.Features.Identity.Roles.GetById;

public class GetByIdRoleRequest : RequestBase
{
    public GetByIdRoleRequest() : base("Roles/{Id}") { }

    public Guid Id { get; set; }
} 