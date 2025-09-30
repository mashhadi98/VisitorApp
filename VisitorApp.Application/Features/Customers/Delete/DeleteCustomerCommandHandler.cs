using VisitorApp.Application.Common.Messaging;
using VisitorApp.Contract.Features.Customers.Delete;
using VisitorApp.Domain.Features.Customers.Entities;

namespace VisitorApp.Application.Features.Customers.Delete;

public class DeleteCustomerCommandHandler(
    IRepository<Customer> repository,
    IEnumerable<IValidatorService<DeleteCustomerCommandRequest, DeleteCustomerCommandResponse>> validators)
    : RequestHandlerBase<DeleteCustomerCommandRequest, DeleteCustomerCommandResponse>(validators)
{
    public override async Task<DeleteCustomerCommandResponse> Handler(DeleteCustomerCommandRequest request, CancellationToken cancellationToken)
    {
        var customer = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (customer == null)
        {
            throw new ArgumentException("مشتری یافت نشد", nameof(request.Id));
        }

        await repository.DeleteAsync(entity: customer, autoSave: true, cancellationToken: cancellationToken);

        var result = new DeleteCustomerCommandResponse
        {
            Success = true,
            Message = "مشتری با موفقیت حذف شد"
        };

        return result;
    }
}
