using VisitorApp.Application.Common.DTOs;
using VisitorApp.Application.Common.Messaging;
using VisitorApp.Domain.Features.Catalog.Entities;

namespace VisitorApp.Application.Features.Catalog.Products.Create;

public class CreateProductCommandHandler(
    IRepository<Product> repository,
    IMapper _mapper,
    IFileStorageService fileStorageService,
    IEnumerable<IValidatorService<CreateProductCommandRequest, CreateProductCommandResponse>> validators) : RequestHandlerBase<CreateProductCommandRequest, CreateProductCommandResponse>(validators)
{
    public override async Task<CreateProductCommandResponse> Handler(CreateProductCommandRequest request, CancellationToken cancellationToken)
    {
        // Create the product entity
        var item = new Product(request.Title, request.Description ?? string.Empty, request.CategoryId)
        {
            IsActive = request.IsActive,
            Price = request.Price
        };

        // Handle image upload if provided
        if (request.ImageFile != null)
        {
            var uploadResult = await UploadProductImage(request, item.Id, cancellationToken);
            if (uploadResult.IsSuccess)
            {
                item.UpdateImage(
                    uploadResult.FilePath!,
                    uploadResult.FileUrl!,
                    uploadResult.FileName,
                    uploadResult.FileSize
                );
            }
        }

        await repository.AddAsync(entity: item, autoSave: true, cancellationToken: cancellationToken);

        var result = new CreateProductCommandResponse
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            IsActive = item.IsActive,
            CategoryId = item.CategoryId,
            Price = item.Price,
            ImageUrl = item.ImageUrl,
            ImageFileName = item.ImageFileName,
            ImageFileSize = item.ImageFileSize,
            HasImage = item.HasImage
        };

        return result;
    }

    private async Task<FileUploadResult> UploadProductImage(CreateProductCommandRequest request, Guid productId, CancellationToken cancellationToken)
    {
                var fileUploadRequest = new FileUploadRequest
        {
            File = request.ImageFile!,
            Folder = $"products/{productId}",
            ExpectedFileType = FileType.Image,
            GenerateUniqueFileName = true,
            
            // Simple upload - no resize or thumbnail
            ShouldResize = false,
            CreateThumbnail = false,
            
            // Security settings
            MaxFileSize = 5 * 1024 * 1024, // 5MB
            AllowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" }
        };

        return await fileStorageService.UploadFileAsync(fileUploadRequest, cancellationToken);
    }
}