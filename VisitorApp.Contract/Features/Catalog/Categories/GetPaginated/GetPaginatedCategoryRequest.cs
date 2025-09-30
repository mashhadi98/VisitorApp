using VisitorApp.Contract.Common;

namespace VisitorApp.Contract.Features.Catalog.Categories.GetPaginated;

public class GetPaginatedCategoryRequest() : PaginatedRequestBase("Categories", ApiTypes.Get)
{
}
