using VisitorApp.Application.Common.Messaging;

namespace VisitorApp.Application.Features.Identity.Users.GetById;

public class GetByIdUserQueryRequest : IRequestBase<GetByIdUserQueryResponse>
{
    public Guid Id { get; set; }
} 