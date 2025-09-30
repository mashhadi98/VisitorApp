using VisitorApp.Application.Features.Orders.ConfirmOrder;
using VisitorApp.Contract.Features.Orders.ConfirmOrder;

namespace VisitorApp.API.Features.Orders.ConfirmOrder;

public class ConfirmOrderHandler : PostEndpoint<ConfirmOrderRequest, ConfirmOrderCommandRequest, ConfirmOrderCommandResponse>
{
    public override string? RolesAccess => "Admin";
    
    public ConfirmOrderHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
}

