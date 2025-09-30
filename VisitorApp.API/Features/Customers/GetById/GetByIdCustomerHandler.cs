using VisitorApp.Application.Features.Customers.GetById;
using VisitorApp.Contract.Features.Customers.GetById;

namespace VisitorApp.API.Features.Customers.GetById;

public class GetByIdCustomerHandler : GetEndpoint<GetByIdCustomerRequest, GetByIdCustomerQueryRequest, GetByIdCustomerQueryResponse>
{
    public override string? RolesAccess => "Admin";
    
    public GetByIdCustomerHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
}
