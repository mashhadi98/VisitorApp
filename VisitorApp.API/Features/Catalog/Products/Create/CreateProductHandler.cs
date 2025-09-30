using VisitorApp.Application.Features.Catalog.Products.Create;
using VisitorApp.Contract.Features.Catalog.Products.Create;

namespace VisitorApp.API.Features.Catalog.Products.Create;

public class CreateProductHandler : PostEndpoint<CreateProductRequest, CreateProductCommandRequest, CreateProductCommandResponse>
{
    public override string? RolesAccess => "";
    
    public CreateProductHandler(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }

    public override void Configure()
    {
        base.Configure();
        
        // Configure to accept multipart/form-data for file uploads
        AllowFormData();
        AllowFileUploads();
    }
}