using FluentValidation;
using VisitorApp.Application.Common.Messaging;
using VisitorApp.Application.Features.Catalog.Products.Create;
using VisitorApp.Contract.Features.Customers.Update;
using VisitorApp.Domain.Common.ResponseMessages;

namespace VisitorApp.Application.Features.Customers.Update.Validators;

public class UpdateCustomerCommandValidator : IValidatorService<UpdateCustomerCommandRequest, UpdateCustomerCommandResponse>
{
    public void Execute(UpdateCustomerCommandRequest request)
    {

    }
}
