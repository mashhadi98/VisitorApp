using VisitorApp.Application.Common.Messaging;

namespace VisitorApp.Application.Features.Catalog.Categories.Delete.Validators;

public class DeleteCategoryCommandValidator : IValidatorService<DeleteCategoryCommandRequest, DeleteCategoryCommandResponse>
{
    public void Execute(DeleteCategoryCommandRequest request)
    {
        if (request.Id == Guid.Empty)
        {
            throw new ArgumentException("شناسه دسته بندی اجباری است");
        }
    }
} 