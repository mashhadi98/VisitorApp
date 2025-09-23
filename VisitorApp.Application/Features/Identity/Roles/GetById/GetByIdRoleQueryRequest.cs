using VisitorApp.Application.Common.Messaging;

namespace VisitorApp.Application.Features.Identity.Roles.GetById;

public class GetByIdRoleQueryRequest : IRequestBase<GetByIdRoleQueryResponse>
{
    public Guid Id { get; set; }
} 