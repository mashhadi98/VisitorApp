using VisitorApp.Application.Features.Orders.GetPaginated;
using VisitorApp.Contract.Features.Orders.GetPaginated;

namespace VisitorApp.API.Features.Orders.GetPaginated;

public class GetPaginatedOrderHandler : GetEndpoint<GetPaginatedOrderRequest, GetPaginatedOrderQueryRequest, PaginatedResponse<GetPaginatedOrderQueryResponse>>
{
    public override string? RolesAccess => "Admin";
    
    public GetPaginatedOrderHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
}
