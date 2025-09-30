namespace VisitorApp.Contract.Features.Dashboard.Summary;

public class GetDashboardSummaryQueryResponse
{
    public int UsersCount { get; set; }
    public int ProductsCount { get; set; }
    public int OrdersThisMonth { get; set; }
    public decimal SalesThisMonth { get; set; }
}

