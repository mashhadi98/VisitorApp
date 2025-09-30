using VisitorApp.Contract.Features.Dashboard.Summary;

namespace VisitorApp.Application.Features.Dashboard.Summary;

public class GetDashboardSummaryQueryRequest : IRequestBase<GetDashboardSummaryQueryResponse>
{
    public string? Month { get; set; }
}

