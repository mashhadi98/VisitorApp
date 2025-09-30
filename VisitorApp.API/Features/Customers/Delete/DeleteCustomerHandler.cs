using VisitorApp.Application.Features.Customers.Delete;
using VisitorApp.Contract.Features.Customers.Delete;

namespace VisitorApp.API.Features.Customers.Delete;

public class DeleteCustomerHandler : DeleteEndpoint<DeleteCustomerRequest, DeleteCustomerCommandRequest, DeleteCustomerCommandResponse>
{
    public override string? RolesAccess => "Admin";
    
    public DeleteCustomerHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
}
