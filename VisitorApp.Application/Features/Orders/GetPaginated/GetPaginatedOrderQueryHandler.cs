using Microsoft.EntityFrameworkCore;
using VisitorApp.Contract.Features.Orders.GetPaginated;
using VisitorApp.Domain.Features.Orders.Entities;

namespace VisitorApp.Application.Features.Orders.GetPaginated;

public class GetPaginatedOrderQueryHandler(IRepository<Order> repository) 
    : RequestHandlerBase<GetPaginatedOrderQueryRequest, PaginatedResponse<GetPaginatedOrderQueryResponse>>()
{
    public override Task<PaginatedResponse<GetPaginatedOrderQueryResponse>> Handler(GetPaginatedOrderQueryRequest request, CancellationToken cancellationToken)
    {
        Expression<Func<Order, bool>> predicate = x => true;
        
        if (request.Filter != null)
        {
            if (!string.IsNullOrWhiteSpace(request.Filter.OrderNumber))
            {
                predicate = predicate.And(o => o.OrderNumber.Contains(request.Filter.OrderNumber));
            }

            if (request.Filter.CustomerId.HasValue)
            {
                predicate = predicate.And(o => o.CustomerId == request.Filter.CustomerId.Value);
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

        var result = repository.GetListPaginatedAsync(request, predicate, x => new GetPaginatedOrderQueryResponse
        {
            Id = x.Id,
            OrderNumber = x.OrderNumber,
            CustomerId = x.CustomerId,
            CustomerName = x.Customer != null ? x.Customer.FullName : null,
            UserId = x.UserId,
            UserFullName = x.User != null ? x.User.FullName : null,
            OrderDate = x.OrderDate,
            TotalAmount = x.TotalAmount,
            Status = (int)x.Status,
            StatusText = x.Status.ToString(),
            ItemCount = x.Items.Count
        }, cancellationToken);

        return result;
    }
}
