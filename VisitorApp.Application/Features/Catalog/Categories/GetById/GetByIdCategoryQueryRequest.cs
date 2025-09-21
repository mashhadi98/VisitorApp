namespace VisitorApp.Application.Features.Catalog.Categories.GetById;

public class GetByIdCategoryQueryRequest : IRequestBase<GetByIdCategoryQueryResponse>
{
    public Guid Id { get; set; }
} 