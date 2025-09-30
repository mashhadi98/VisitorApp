using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VisitorApp.Contract.Features.Dashboard.Summary;
using VisitorApp.Domain.Features.Catalog.Entities;
using VisitorApp.Domain.Features.Identity.Entities;
using VisitorApp.Domain.Features.Orders.Entities;

namespace VisitorApp.Application.Features.Dashboard.Summary;

public class GetDashboardSummaryQueryHandler(
    UserManager<ApplicationUser> userManager,
    IRepository<Product> productRepository,
    IRepository<Order> orderRepository) 
    : RequestHandlerBase<GetDashboardSummaryQueryRequest, GetDashboardSummaryQueryResponse>()
{
    public override async Task<GetDashboardSummaryQueryResponse> Handler(GetDashboardSummaryQueryRequest request, CancellationToken cancellationToken)
    {
        // Parse month parameter or use current month
        DateTime startDate;
        DateTime endDate;

        if (!string.IsNullOrEmpty(request.Month) && DateTime.TryParse($"{request.Month}-01", out var parsedDate))
        {
            startDate = new DateTime(parsedDate.Year, parsedDate.Month, 1);
            endDate = startDate.AddMonths(1).AddDays(-1);
        }
        else
        {
            var now = DateTime.UtcNow;
            startDate = new DateTime(now.Year, now.Month, 1);
            endDate = startDate.AddMonths(1).AddDays(-1);
        }

        // Count users
        var usersCount = await userManager.Users.CountAsync(cancellationToken);

        // Count products
        var productsCount = await productRepository.GetQuery().CountAsync(cancellationToken);

        // Count orders this month
        var ordersThisMonth = await orderRepository.GetQuery()
            .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
            .CountAsync(cancellationToken);

        // Sum sales this month
        var salesThisMonth = await orderRepository.GetQuery()
            .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
            .SumAsync(o => (decimal?)o.TotalAmount, cancellationToken) ?? 0;

        var result = new GetDashboardSummaryQueryResponse
        {
            UsersCount = usersCount,
            ProductsCount = productsCount,
            OrdersThisMonth = ordersThisMonth,
            SalesThisMonth = salesThisMonth
        };

        return result;
    }
}

