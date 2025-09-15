namespace VisitorApp.Application.Features.Catalog.Products.ChangeState;

public class ChangeStateProductCommandRequest : IRequestBase<ChangeStateProductCommandResponse>
{
    public Guid Id { get; set; }
}