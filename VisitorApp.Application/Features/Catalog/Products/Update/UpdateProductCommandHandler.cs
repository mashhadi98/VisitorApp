using VisitorApp.Application.Common.DTOs;
using VisitorApp.Application.Common.Messaging;
using VisitorApp.Contract.Features.Catalog.Products.Update;
using VisitorApp.Domain.Features.Catalog.Entities;

namespace VisitorApp.Application.Features.Catalog.Products.Update;

public class UpdateProductCommandHandler(
    IRepository<Product> repository,
    IMapper _mapper,
    IFileStorageService fileStorageService,
    IEnumerable<IValidatorService<UpdateProductCommandRequest, UpdateProductCommandResponse>> validators) : RequestHandlerBase<UpdateProductCommandRequest, UpdateProductCommandResponse>(validators)
{
    public override async Task<UpdateProductCommandResponse> Handler(UpdateProductCommandRequest request, CancellationToken cancellationToken)
    {
        var item = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (item == null)
        {
            throw new ArgumentException("Product not found", nameof(request.Id));
        }

        // Update basic properties
        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            item.Title = request.Title;
        }
        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            item.Description = request.Description;
        }
        if (request.IsActive != null)
        {
            item.IsActive = request.IsActive ?? false;
        }
        if (request.CategoryId != null)
        {
            item.CategoryId = request.CategoryId;
        }
        if (request.Price != null)
        {
            item.Price = request.Price.Value;
        }

        // Handle image operations
        await HandleImageOperations(item, request, cancellationToken);

        await repository.UpdateAsync(entity: item, autoSave: true, cancellationToken: cancellationToken);

        var result = new UpdateProductCommandResponse
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

    private async Task HandleImageOperations(Product product, UpdateProductCommandRequest request, CancellationToken cancellationToken)
    {
        // Remove existing image if requested
        if (request.RemoveExistingImage == true && product.HasImage)
        {
            await RemoveProductImage(product, cancellationToken);
        }

        // Upload new image if provided
        if (request.ImageFile != null)
        {
            // Remove existing image first if it exists
            if (product.HasImage)
            {
                await RemoveProductImage(product, cancellationToken);
            }

            // Upload new image
            var uploadResult = await UploadProductImage(request, product.Id, cancellationToken);
            if (uploadResult.IsSuccess)
            {
                product.UpdateImage(
                    uploadResult.FilePath!,
                    uploadResult.FileUrl!,
                    uploadResult.FileName,
                    uploadResult.FileSize
                );
            }
        }
    }

    private async Task RemoveProductImage(Product product, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(product.ImagePath))
        {
            await fileStorageService.DeleteFileAsync(product.ImagePath, cancellationToken);
        }



        product.RemoveImage();
    }

    private async Task<FileUploadResult> UploadProductImage(UpdateProductCommandRequest request, Guid productId, CancellationToken cancellationToken)
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