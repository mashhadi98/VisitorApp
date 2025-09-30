using VisitorApp.Contract.Features.Orders.Delete;

namespace VisitorApp.Application.Features.Orders.Delete;

public class DeleteOrderCommandRequest : IRequestBase<DeleteOrderCommandResponse>
{
    public Guid Id { get; set; }
}
