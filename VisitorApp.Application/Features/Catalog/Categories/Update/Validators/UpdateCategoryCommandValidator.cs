using VisitorApp.Application.Common.Messaging;
using VisitorApp.Domain.Common.ResponseMessages;

namespace VisitorApp.Application.Features.Catalog.Categories.Update.Validators;

public class UpdateCategoryCommandValidator : IValidatorService<UpdateCategoryCommandRequest, UpdateCategoryCommandResponse>
{
    public void Execute(UpdateCategoryCommandRequest request)
    {
        if (request.Id == Guid.Empty)
        {
            throw new ArgumentException("شناسه دسته بندی اجباری است");
        }
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException(ErrorMessages.Categories.NameNotFound);
        }
        if (request.Name.Length > 200)
        {
            throw new ArgumentException("نام دسته بندی نمی تواند بیش از 200 کاراکتر باشد");
        }
        if (!string.IsNullOrWhiteSpace(request.Description) && request.Description.Length > 1000)
        {
            throw new ArgumentException("توضیحات دسته بندی نمی تواند بیش از 1000 کاراکتر باشد");
        }
    }
} 