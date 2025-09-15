using Shared.Helpers;

namespace VisitorApp.API.Common.Models;

public class RequestBase
{
    protected RequestBase(string route)
    {
        var routItems = route.Split("/");
        var routResult = "";
        foreach (var item in routItems)
        {
            if (item.IndexOf("{") > -1)
            {
                routResult += $"/{item}";
            }
            else
            {
                routResult += $"/{item.ToSnakeCase()}";
            }
        }
        Route = routResult;
    }
    public string Route { get; private set; }
}
public class PaginatedRequestBase : RequestBase
{
    protected PaginatedRequestBase(string route) : base(route) { }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
public class PaginatedRequestBase<TFilter> : PaginatedRequestBase where TFilter : new()
{
    protected PaginatedRequestBase(string route) : base(route) { }

    [FromQuery]
    public TFilter Filter { get; set; } = new TFilter();

    public Sort? Sort { get; set; }
}
public enum Sort
{
    CraeteDate = 1,
    DescendingCraeteDate = 10,
    Letters = 2,
    DescendingLetters = 20,
}