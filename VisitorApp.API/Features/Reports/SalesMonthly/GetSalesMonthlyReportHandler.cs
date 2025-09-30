using VisitorApp.API.Common.Endpoints;
using VisitorApp.Contract.Features.Reports.SalesMonthly;
using VisitorApp.Application.Features.Reports.SalesMonthly;

namespace VisitorApp.API.Features.Reports.SalesMonthly;

public class GetSalesMonthlyReportHandler : GetEndpoint<GetSalesMonthlyReportRequest, GetSalesMonthlyReportQuery, GetSalesMonthlyReportQueryResponse>
{
    public override string? RolesAccess => "Admin";

    public GetSalesMonthlyReportHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
}

