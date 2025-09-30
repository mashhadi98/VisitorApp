using VisitorApp.Application.Features.Customers.Update;
using VisitorApp.Contract.Features.Customers.Update;

namespace VisitorApp.API.Features.Customers.Update;

public class UpdateCustomerHandler : PutEndpoint<UpdateCustomerRequest, UpdateCustomerCommandRequest, UpdateCustomerCommandResponse>
{
    public override string? RolesAccess => "Admin";
    
    public UpdateCustomerHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
}
