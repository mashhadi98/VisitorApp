using VisitorApp.Application.Features.Orders.Delete;
using VisitorApp.Contract.Features.Orders.Delete;

namespace VisitorApp.API.Features.Orders.Delete;

public class DeleteOrderHandler : DeleteEndpoint<DeleteOrderRequest, DeleteOrderCommandRequest, DeleteOrderCommandResponse>
{
    public override string? RolesAccess => "Admin";
    
    public DeleteOrderHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
}
