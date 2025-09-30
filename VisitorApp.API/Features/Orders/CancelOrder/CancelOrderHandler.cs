using VisitorApp.Application.Features.Orders.CancelOrder;
using VisitorApp.Contract.Features.Orders.CancelOrder;

namespace VisitorApp.API.Features.Orders.CancelOrder;

public class CancelOrderHandler : PostEndpoint<CancelOrderRequest, CancelOrderCommandRequest, CancelOrderCommandResponse>
{
    public override string? RolesAccess => "Admin";
    
    public CancelOrderHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
}

