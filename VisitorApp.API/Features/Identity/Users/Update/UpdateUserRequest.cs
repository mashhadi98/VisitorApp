namespace VisitorApp.API.Features.Identity.Users.Update;

public class UpdateUserRequest : RequestBase
{
    public UpdateUserRequest() : base("Users/{Id}") { }

    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool? IsActive { get; set; }
} 