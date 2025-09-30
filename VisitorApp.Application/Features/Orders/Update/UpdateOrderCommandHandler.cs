using Microsoft.EntityFrameworkCore;
using VisitorApp.Application.Common.Messaging;
using VisitorApp.Contract.Features.Orders.Common;
using VisitorApp.Contract.Features.Orders.Update;
using VisitorApp.Domain.Features.Catalog.Entities;
using VisitorApp.Domain.Features.Customers.Entities;
using VisitorApp.Domain.Features.Orders.Entities;

namespace VisitorApp.Application.Features.Orders.Update;

public class UpdateOrderCommandHandler(
    IRepository<Order> orderRepository,
    IRepository<OrderItem> orderItemRepository,
    IRepository<Customer> customerRepository,
    IRepository<Product> productRepository,
    IEnumerable<IValidatorService<UpdateOrderCommandRequest, UpdateOrderCommandResponse>> validators)
    : RequestHandlerBase<UpdateOrderCommandRequest, UpdateOrderCommandResponse>(validators)
{
    public override async Task<UpdateOrderCommandResponse> Handler(UpdateOrderCommandRequest request, CancellationToken cancellationToken)
    {
        // Load order with items
        var order = await orderRepository.GetQuery()
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

        if (order == null)
        {
            throw new ArgumentException("سفارش یافت نشد", nameof(request.Id));
        }

        // Verify customer exists
        var customer = await customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer == null)
        {
            throw new ArgumentException("مشتری یافت نشد", nameof(request.CustomerId));
        }

        // Update order properties
        order.CustomerId = request.CustomerId;
        order.OrderDate = request.OrderDate;
        order.Status = (OrderStatus)request.Status;

        // Remove all existing items
        var existingItems = order.Items.ToList();
        foreach (var item in existingItems)
        {
            await orderItemRepository.DeleteAsync(item, autoSave: false, cancellationToken: cancellationToken);
        }
        order.Items.Clear();

        // Add new items
        decimal totalAmount = 0;
        foreach (var itemDto in request.Items)
        {
            // Verify product exists
            var product = await productRepository.GetByIdAsync(itemDto.ProductId, cancellationToken);
            if (product == null)
            {
                throw new ArgumentException($"محصول با شناسه {itemDto.ProductId} یافت نشد");
            }

            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                ProductId = itemDto.ProductId,
                Quantity = itemDto.Quantity,
                UnitPrice = itemDto.UnitPrice
            };

            order.Items.Add(orderItem);
            totalAmount += orderItem.TotalPrice;
        }

        order.TotalAmount = totalAmount;

        await orderRepository.UpdateAsync(entity: order, autoSave: true, cancellationToken: cancellationToken);

        // Load relations for response
        var savedOrder = await orderRepository.GetQuery()
            .Include(o => o.Customer)
            .Include(o => o.User)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == order.Id, cancellationToken);

        var result = new UpdateOrderCommandResponse
        {
            Id = savedOrder!.Id,
            OrderNumber = savedOrder.OrderNumber,
            CustomerId = savedOrder.CustomerId,
            CustomerName = savedOrder.Customer?.FullName,
            UserId = savedOrder.UserId,
            UserFullName = savedOrder.User?.FullName,
            OrderDate = savedOrder.OrderDate,
            TotalAmount = savedOrder.TotalAmount,
            Status = (int)savedOrder.Status,
            Items = savedOrder.Items.Select(i => new OrderItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductTitle = i.Product?.Title,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };

        return result;
    }
}
