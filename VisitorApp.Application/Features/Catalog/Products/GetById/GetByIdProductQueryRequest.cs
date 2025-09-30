using VisitorApp.Contract.Features.Catalog.Products.GetById;

namespace VisitorApp.Application.Features.Catalog.Products.GetById;

public class GetByIdProductQueryRequest : IRequestBase<GetByIdProductQueryResponse>
{
    public Guid Id { get; set; }
}