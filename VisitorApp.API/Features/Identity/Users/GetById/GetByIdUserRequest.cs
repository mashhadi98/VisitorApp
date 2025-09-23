namespace VisitorApp.API.Features.Identity.Users.GetById;

public class GetByIdUserRequest : RequestBase
{
    public GetByIdUserRequest() : base("Users/{Id}") { }

    public Guid Id { get; set; }
} 