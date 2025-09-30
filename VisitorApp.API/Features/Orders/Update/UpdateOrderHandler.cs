using VisitorApp.Application.Features.Orders.Update;
using VisitorApp.Contract.Features.Orders.Update;

namespace VisitorApp.API.Features.Orders.Update;

public class UpdateOrderHandler : PutEndpoint<UpdateOrderRequest, UpdateOrderCommandRequest, UpdateOrderCommandResponse>
{
    public override string? RolesAccess => "Admin";
    
    public UpdateOrderHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
}
