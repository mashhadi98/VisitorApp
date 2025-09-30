using VisitorApp.Application.Features.Dashboard.Summary;
using VisitorApp.Contract.Features.Dashboard.Summary;

namespace VisitorApp.API.Features.Dashboard.Summary;

public class GetDashboardSummaryHandler : GetEndpoint<GetDashboardSummaryRequest, GetDashboardSummaryQueryRequest, GetDashboardSummaryQueryResponse>
{
    public override string? RolesAccess => "Admin";
    
    public GetDashboardSummaryHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
}

