using VisitorApp.Application.Abstractions.Messaging;
using VisitorApp.Contract.Features.Reports.SalesMonthly;

namespace VisitorApp.Application.Features.Reports.SalesMonthly;

// Application layer wrapper for MediatR
public class GetSalesMonthlyReportQuery : IRequestBase<GetSalesMonthlyReportQueryResponse>
{
    public int Months { get; set; } = 6;
    public int? Year { get; set; }
}
