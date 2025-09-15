namespace VisitorApp.Application.Features.Catalog.Products.GetById;

public class GetByIdProductQueryRequest : IRequestBase<GetByIdProductQueryResponse>
{
    public Guid Id { get; set; }
}