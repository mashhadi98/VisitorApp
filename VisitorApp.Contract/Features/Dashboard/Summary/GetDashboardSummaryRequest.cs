using VisitorApp.Contract.Common;

namespace VisitorApp.Contract.Features.Dashboard.Summary;

public class GetDashboardSummaryRequest() : RequestBase("dashboard/summary", ApiTypes.Get)
{
    public string? Month { get; set; } // مثال: "2025-09"
}

