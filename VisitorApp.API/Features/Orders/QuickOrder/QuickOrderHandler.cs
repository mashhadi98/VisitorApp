using VisitorApp.Application.Features.Orders.QuickOrder;
using VisitorApp.Contract.Features.Orders.QuickOrder;

namespace VisitorApp.API.Features.Orders.QuickOrder;

public class QuickOrderHandler : PostEndpoint<QuickOrderRequest, QuickOrderCommandRequest, QuickOrderCommandResponse>
{
    public override string? RolesAccess => "Admin";
    
    public QuickOrderHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
}

