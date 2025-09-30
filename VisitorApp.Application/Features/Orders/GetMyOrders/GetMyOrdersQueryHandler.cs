using Microsoft.EntityFrameworkCore;
using VisitorApp.Application.Common.Services;
using VisitorApp.Contract.Features.Orders.GetMyOrders;
using VisitorApp.Domain.Features.Orders.Entities;

namespace VisitorApp.Application.Features.Orders.GetMyOrders;

public class GetMyOrdersQueryHandler(
    IRepository<Order> repository,
    ICurrentUserService currentUserService) 
    : RequestHandlerBase<GetMyOrdersQueryRequest, PaginatedResponse<GetMyOrdersQueryResponse>>()
{
    public override Task<PaginatedResponse<GetMyOrdersQueryResponse>> Handler(GetMyOrdersQueryRequest request, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId;
        
        if (!currentUserId.HasValue)
        {
            throw new UnauthorizedAccessException("کاربر وارد سیستم نشده است");
        }

        // فیلتر اولیه: فقط سفارشات کاربر جاری
        Expression<Func<Order, bool>> predicate = x => x.UserId == currentUserId.Value;
        
        if (request.Filter != null)
        {
            if (!string.IsNullOrWhiteSpace(request.Filter.OrderNumber))
            {
                predicate = predicate.And(o => o.OrderNumber.Contains(request.Filter.OrderNumber));
            }

            if (request.Filter.Status.HasValue)
            {
                predicate = predicate.And(o => (int)o.Status == request.Filter.Status.Value);
            }

            if (request.Filter.OrderDateFrom.HasValue)
            {
                predicate = predicate.And(o => o.OrderDate >= request.Filter.OrderDateFrom.Value);
            }

            if (request.Filter.OrderDateTo.HasValue)
            {
                predicate = predicate.And(o => o.OrderDate <= request.Filter.OrderDateTo.Value);
            }
        }

        var result = repository.GetListPaginatedAsync(request, predicate, x => new GetMyOrdersQueryResponse
        {
            Id = x.Id,
            OrderNumber = x.OrderNumber,
            CustomerId = x.CustomerId,
            CustomerName = x.Customer != null ? x.Customer.FullName : null,
            OrderDate = x.OrderDate,
            TotalAmount = x.TotalAmount,
            Status = (int)x.Status,
            StatusText = x.Status.ToString(),
            ItemCount = x.Items.Count
        }, cancellationToken);

        return result;
    }
}
