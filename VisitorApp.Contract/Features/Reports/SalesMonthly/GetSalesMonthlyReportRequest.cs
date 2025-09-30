using VisitorApp.Contract.Common;

namespace VisitorApp.Contract.Features.Reports.SalesMonthly;

public class GetSalesMonthlyReportRequest() : RequestBase("reports/sales/monthly", ApiTypes.Get)
{
    public int Months { get; set; } = 6; // تعداد ماه گذشته
    public int? Year { get; set; } // مثلا 2025
}

