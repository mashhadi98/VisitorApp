using VisitorApp.API.Common.Models;

namespace VisitorApp.API.Features.Identity.Register;

public class RegisterRequest() : RequestBase("Identity/Register")
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
} 