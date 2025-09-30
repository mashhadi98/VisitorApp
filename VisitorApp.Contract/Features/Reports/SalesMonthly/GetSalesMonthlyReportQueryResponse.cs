namespace VisitorApp.Contract.Features.Reports.SalesMonthly;

public class GetSalesMonthlyReportQueryResponse
{
    public List<MonthlySalesData> MonthlyData { get; set; } = new();
}

public class MonthlySalesData
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public decimal TotalSales { get; set; }
    public int OrderCount { get; set; }
}
