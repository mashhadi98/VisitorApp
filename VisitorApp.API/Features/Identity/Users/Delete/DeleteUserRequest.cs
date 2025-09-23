namespace VisitorApp.API.Features.Identity.Users.Delete;

public class DeleteUserRequest : RequestBase
{
    public DeleteUserRequest() : base("Users/{Id}") { }

    public Guid Id { get; set; }
} 