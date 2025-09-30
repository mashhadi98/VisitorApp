using VisitorApp.Contract.Features.Catalog.Products.ChangeState;

namespace VisitorApp.Application.Features.Catalog.Products.ChangeState;

public class ChangeStateProductCommandRequest : IRequestBase<ChangeStateProductCommandResponse>
{
    public Guid Id { get; set; }
}