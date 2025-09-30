using VisitorApp.Application.Common.Messaging;
using VisitorApp.Contract.Features.Orders.Delete;

namespace VisitorApp.Application.Features.Orders.Delete.Validators;

public class DeleteOrderCommandValidator : IValidatorService<DeleteOrderCommandRequest, DeleteOrderCommandResponse>
{
    public void Execute(DeleteOrderCommandRequest request)
    {

    }
}
