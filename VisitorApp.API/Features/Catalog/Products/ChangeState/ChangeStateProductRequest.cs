namespace VisitorApp.API.Features.Catalog.Products.ChangeState;

public class ChangeStateProductRequest() : RequestBase("Products/ChangeState")
{
    [QueryParam]
    public Guid Id { get; set; }
}