using VisitorApp.Application.Common.Messaging;
using VisitorApp.Contract.Features.Orders.ConfirmOrder;
using VisitorApp.Domain.Features.Orders.Entities;

namespace VisitorApp.Application.Features.Orders.ConfirmOrder;

public class ConfirmOrderCommandHandler(
    IRepository<Order> orderRepository,
    IEnumerable<IValidatorService<ConfirmOrderCommandRequest, ConfirmOrderCommandResponse>> validators) 
    : RequestHandlerBase<ConfirmOrderCommandRequest, ConfirmOrderCommandResponse>(validators)
{
    public override async Task<ConfirmOrderCommandResponse> Handler(ConfirmOrderCommandRequest request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (order == null)
        {
            throw new ArgumentException("سفارش یافت نشد", nameof(request.Id));
        }

        if (order.Status == OrderStatus.Confirmed)
        {
            throw new InvalidOperationException("این سفارش قبلاً تایید شده است");
        }

        if (order.Status == OrderStatus.Canceled)
        {
            throw new InvalidOperationException("سفارش لغو شده قابل تایید نیست");
        }

        order.Status = OrderStatus.Confirmed;
        await orderRepository.UpdateAsync(entity: order, autoSave: true, cancellationToken: cancellationToken);

        var result = new ConfirmOrderCommandResponse
        {
            Success = true,
            Message = "سفارش با موفقیت تایید شد"
        };

        return result;
    }
}

