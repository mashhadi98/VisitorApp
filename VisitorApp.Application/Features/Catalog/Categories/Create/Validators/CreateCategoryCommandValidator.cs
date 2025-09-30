using VisitorApp.Application.Common.Messaging;
using VisitorApp.Contract.Features.Catalog.Categories.Create;
using VisitorApp.Domain.Common.ResponseMessages;

namespace VisitorApp.Application.Features.Catalog.Categories.Create.Validators;

public class CreateCategoryCommandValidator : IValidatorService<CreateCategoryCommandRequest, CreateCategoryCommandResponse>
{
    public void Execute(CreateCategoryCommandRequest request)
    {
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