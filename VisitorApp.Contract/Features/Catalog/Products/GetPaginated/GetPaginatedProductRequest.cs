using VisitorApp.Contract.Common;

namespace VisitorApp.Contract.Features.Catalog.Products.GetPaginated;

public class GetPaginatedProductRequest() : PaginatedRequestBase("Products", ApiTypes.Get)
{
}
