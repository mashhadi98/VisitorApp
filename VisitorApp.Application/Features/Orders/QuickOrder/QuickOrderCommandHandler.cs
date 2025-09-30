using Microsoft.EntityFrameworkCore;
using VisitorApp.Application.Common.Services;
using VisitorApp.Contract.Features.Orders.QuickOrder;
using VisitorApp.Domain.Features.Customers.Entities;
using VisitorApp.Domain.Features.Orders.Entities;
using VisitorApp.Domain.Features.Catalog.Entities;
using VisitorApp.Application.Common.Messaging;

namespace VisitorApp.Application.Features.Orders.QuickOrder;

public class QuickOrderCommandHandler(
    IRepository<Order> orderRepository,
    IRepository<Customer> customerRepository,
    IRepository<Product> productRepository,
    ICurrentUserService currentUserService,
    IEnumerable<IValidatorService<QuickOrderCommandRequest, QuickOrderCommandResponse>> validators) 
    : RequestHandlerBase<QuickOrderCommandRequest, QuickOrderCommandResponse>(validators)
{
    public override async Task<QuickOrderCommandResponse> Handler(QuickOrderCommandRequest request, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId;
        
        // اگر UserId ارسال شده، بررسی دسترسی
        Guid? orderUserId;
        if (request.UserId.HasValue)
        {
            // فقط Admin می‌تواند سفارش را به نام کاربر دیگر ثبت کند
            // TODO: بررسی نقش کاربر (این قسمت را بر اساس سیستم نقش‌ها تنظیم کنید)
            orderUserId = request.UserId.Value;
        }
        else
        {
            orderUserId = currentUserId;
        }

        // مدیریت مشتری
        Guid customerId;
        if (request.CustomerId.HasValue)
        {
            // استفاده از مشتری موجود
            var existingCustomer = await customerRepository.GetByIdAsync(request.CustomerId.Value, cancellationToken);
            if (existingCustomer == null)
            {
                throw new ArgumentException("مشتری یافت نشد", nameof(request.CustomerId));
            }
            customerId = request.CustomerId.Value;
        }
        else if (request.NewCustomer != null)
        {
            // ایجاد مشتری جدید
            var newCustomer = new Customer
            {
                FullName = request.NewCustomer.FullName,
                PhoneNumber = request.NewCustomer.PhoneNumber,
                CompanyName = request.NewCustomer.CompanyName,
                IsTemporary = request.NewCustomer.IsTemporary
            };
            await customerRepository.AddAsync(newCustomer, autoSave: true, cancellationToken: cancellationToken);
            customerId = newCustomer.Id;
        }
        else
        {
            throw new ArgumentException("باید CustomerId یا NewCustomer ارسال شود");
        }

        // Generate order number
        var orderNumber = await GenerateOrderNumber(cancellationToken);

        // Create order
        var order = new Order
        {
            OrderNumber = orderNumber,
            CustomerId = customerId,
            UserId = orderUserId,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Pending,
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

        var result = new QuickOrderCommandResponse
        {
            OrderId = order.Id,
            OrderNumber = order.OrderNumber,
            UserId = order.UserId,
            Status = (int)order.Status,
            TotalAmount = order.TotalAmount
        };

        return result;
    }

    private async Task<string> GenerateOrderNumber(CancellationToken cancellationToken)
    {
        var orderCount = await orderRepository.GetQuery().CountAsync(cancellationToken);
        var nextNumber = orderCount + 1;
        return $"#{nextNumber:D5}";
    }
}

