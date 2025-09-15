using VisitorApp.Domain.Features.Catalog.Entities;

namespace VisitorApp.Application.Features.Catalog.Products.GetDropdown;

public class GetDropdownProductQueryHandler(IRepository<Product> repository) : RequestHandlerBase<GetDropdownProductQueryRequest, List<DropDownDto>>()
{
    public override Task<List<DropDownDto>> Handler(GetDropdownProductQueryRequest request, CancellationToken cancellationToken)
    {
        var result = repository.GetDropDownAsync(x => new DropDownDto
        {
            Id = x.Id,
            Title = x.Title,
        }, cancellationToken);

        return result;
    }
}