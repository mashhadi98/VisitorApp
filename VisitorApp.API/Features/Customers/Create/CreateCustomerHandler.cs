using VisitorApp.Application.Features.Customers.Create;
using VisitorApp.Contract.Features.Customers.Create;

namespace VisitorApp.API.Features.Customers.Create;

public class CreateCustomerHandler : PostEndpoint<CreateCustomerRequest, CreateCustomerCommandRequest, CreateCustomerCommandResponse>
{
    public override string? RolesAccess => "Admin";
    
    public CreateCustomerHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
}
