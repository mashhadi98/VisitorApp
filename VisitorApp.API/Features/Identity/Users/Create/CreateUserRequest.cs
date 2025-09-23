namespace VisitorApp.API.Features.Identity.Users.Create;

public class CreateUserRequest : RequestBase
{
    public CreateUserRequest() : base("Users") { }

    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; } = true;
} 