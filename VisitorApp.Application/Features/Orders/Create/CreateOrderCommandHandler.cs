using Microsoft.EntityFrameworkCore;
using VisitorApp.Application.Common.Services;
using VisitorApp.Contract.Features.Orders.Common;
using VisitorApp.Contract.Features.Orders.Create;
using VisitorApp.Domain.Features.Customers.Entities;
using VisitorApp.Domain.Features.Orders.Entities;
using VisitorApp.Domain.Features.Catalog.Entities;
using VisitorApp.Application.Common.Messaging;

namespace VisitorApp.Application.Features.Orders.Create;

public class CreateOrderCommandHandler(
    IRepository<Order> orderRepository,
    IRepository<Customer> customerRepository,
    IRepository<Product> productRepository,
    ICurrentUserService currentUserService,
    IEnumerable<IValidatorService<CreateOrderCommandRequest, CreateOrderCommandResponse>> validators) 
    : RequestHandlerBase<CreateOrderCommandRequest, CreateOrderCommandResponse>(validators)
{
    public override async Task<CreateOrderCommandResponse> Handler(CreateOrderCommandRequest request, CancellationToken cancellationToken)
    {
        // Verify customer exists
        var customer = await customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer == null)
        {
            throw new ArgumentException("مشتری یافت نشد", nameof(request.CustomerId));
        }

        // Generate order number
        var orderNumber = await GenerateOrderNumber(cancellationToken);

        // Create order
        var order = new Order
        {
            OrderNumber = orderNumber,
            CustomerId = request.CustomerId,
            UserId = currentUserService.UserId, // ثبت کاربر ایجاد کننده سفارش
            OrderDate = request.OrderDate,
            Status = (OrderStatus)request.Status,
            TotalAmount = 0
        };

        // Add order items
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

        await orderRepository.AddAsync(entity: order, autoSave: true, cancellationToken: cancellationToken);

        // Load relations for response
        var savedOrder = await orderRepository.GetQuery()
            .Include(o => o.Customer)
            .Include(o => o.User)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == order.Id, cancellationToken);

        var result = new CreateOrderCommandResponse
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

    private async Task<string> GenerateOrderNumber(CancellationToken cancellationToken)
    {
        var lastOrder = await orderRepository.GetQuery()
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        var orderCount = await orderRepository.GetQuery().CountAsync(cancellationToken);
        var nextNumber = orderCount + 1;

        return $"#{nextNumber:D5}";
    }
}
