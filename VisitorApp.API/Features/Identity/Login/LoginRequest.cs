namespace VisitorApp.API.Features.Identity.Login;

public class LoginRequest() : RequestBase("Identity/Login")
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; } = false;
}