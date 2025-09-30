using Microsoft.EntityFrameworkCore;
using VisitorApp.Application.Common.Messaging;
using VisitorApp.Contract.Features.Orders.Delete;
using VisitorApp.Domain.Features.Orders.Entities;

namespace VisitorApp.Application.Features.Orders.Delete;

public class DeleteOrderCommandHandler(
    IRepository<Order> orderRepository,
    IRepository<OrderItem> orderItemRepository,
    IEnumerable<IValidatorService<DeleteOrderCommandRequest, DeleteOrderCommandResponse>> validators) 
    : RequestHandlerBase<DeleteOrderCommandRequest, DeleteOrderCommandResponse>(validators)
{
    public override async Task<DeleteOrderCommandResponse> Handler(DeleteOrderCommandRequest request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetQuery()
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);
        
        if (order == null)
        {
            throw new ArgumentException("سفارش یافت نشد", nameof(request.Id));
        }

        // Delete all order items first
        foreach (var item in order.Items.ToList())
        {
            await orderItemRepository.DeleteAsync(entity: item, autoSave: false, cancellationToken: cancellationToken);
        }

        // Then delete the order
        await orderRepository.DeleteAsync(entity: order, autoSave: true, cancellationToken: cancellationToken);

        var result = new DeleteOrderCommandResponse
        {
            Success = true,
            Message = "سفارش با موفقیت حذف شد"
        };

        return result;
    }
}
