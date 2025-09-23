namespace VisitorApp.API.Features.Identity.Roles.Create;

public class CreateRoleRequest : RequestBase
{
    public CreateRoleRequest() : base("Roles") { }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
} 