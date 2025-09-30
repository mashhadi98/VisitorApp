namespace VisitorApp.Contract.Features.Reports.SalesMonthly;

public class GetSalesMonthlyReportQueryRequest
{
    public int Months { get; set; } = 6;
    public int? Year { get; set; }
}
