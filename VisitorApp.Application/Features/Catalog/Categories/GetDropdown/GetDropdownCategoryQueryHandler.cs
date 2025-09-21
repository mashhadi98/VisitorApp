using VisitorApp.Domain.Features.Catalog.Entities;

namespace VisitorApp.Application.Features.Catalog.Categories.GetDropdown;

public class GetDropdownCategoryQueryHandler(IRepository<Category> repository) : RequestHandlerBase<GetDropdownCategoryQueryRequest, List<DropDownDto>>()
{
    public override Task<List<DropDownDto>> Handler(GetDropdownCategoryQueryRequest request, CancellationToken cancellationToken)
    {
        var result = repository.GetDropDownAsync(x => new DropDownDto
        {
            Id = x.Id,
            Title = x.Name,
        }, cancellationToken);

        return result;
    }
} 