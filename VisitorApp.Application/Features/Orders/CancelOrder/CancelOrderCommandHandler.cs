using VisitorApp.Application.Common.Messaging;
using VisitorApp.Contract.Features.Orders.CancelOrder;
using VisitorApp.Domain.Features.Orders.Entities;

namespace VisitorApp.Application.Features.Orders.CancelOrder;

public class CancelOrderCommandHandler(
    IRepository<Order> orderRepository,
    IEnumerable<IValidatorService<CancelOrderCommandRequest, CancelOrderCommandResponse>> validators) 
    : RequestHandlerBase<CancelOrderCommandRequest, CancelOrderCommandResponse>(validators)
{
    public override async Task<CancelOrderCommandResponse> Handler(CancelOrderCommandRequest request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (order == null)
        {
            throw new ArgumentException("سفارش یافت نشد", nameof(request.Id));
        }

        if (order.Status == OrderStatus.Canceled)
        {
            throw new InvalidOperationException("این سفارش قبلاً لغو شده است");
        }

        order.Status = OrderStatus.Canceled;
        await orderRepository.UpdateAsync(entity: order, autoSave: true, cancellationToken: cancellationToken);

        var result = new CancelOrderCommandResponse
        {
            Success = true,
            Message = "سفارش با موفقیت لغو شد"
        };

        return result;
    }
}

