using VisitorApp.Application.Abstractions.Messaging;
using VisitorApp.Contract.Features.Reports.SalesMonthly;

namespace VisitorApp.Application.Features.Reports.SalesMonthly;


public class GetSalesMonthlyReportQueryHandler : RequestHandlerBase<GetSalesMonthlyReportQuery, GetSalesMonthlyReportQueryResponse>
{
    public override async Task<GetSalesMonthlyReportQueryResponse> Handler(GetSalesMonthlyReportQuery request, CancellationToken cancellationToken)
    {
        // TODO: پیاده‌سازی واقعی با دیتابیس
        // فعلاً داده‌های Mock برمی‌گرداند
        
        var response = new GetSalesMonthlyReportQueryResponse
        {
            MonthlyData = new List<MonthlySalesData>
            {
                new() { Year = 2025, Month = 9, MonthName = "مهر", TotalSales = 15000000, OrderCount = 45 },
                new() { Year = 2025, Month = 8, MonthName = "شهریور", TotalSales = 12000000, OrderCount = 38 },
                new() { Year = 2025, Month = 7, MonthName = "مرداد", TotalSales = 18000000, OrderCount = 52 },
            }
        };

        return await Task.FromResult(response);
    }
}
