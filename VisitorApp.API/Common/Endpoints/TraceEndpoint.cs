using VisitorApp.API.Common.Models;
using VisitorApp.Application.Abstractions.Messaging;

namespace VisitorApp.API.Common.Endpoints;

public abstract class TraceEndpointWithoutRequest<TResponse> : EndpointWithoutRequestBase<TResponse>
    where TResponse : class
{
    public override ApiTypes Type { get; } = ApiTypes.Trace;

}

public abstract class TraceEndpointWithoutRequest<TRequest, TCommand, TResponse> : EndpointWithoutRequestBase<TRequest, TCommand, TResponse>
    where TResponse : class
    where TRequest : RequestBase, new()
    where TCommand : IRequestBase<TResponse>, new()
{
    protected TraceEndpointWithoutRequest(ISender sender) : base(sender)
    {
    }

    public override ApiTypes Type { get; } = ApiTypes.Trace;

}

public abstract class TraceEndpoint<TRequest, TResponse> : EndpointBase<TRequest, TResponse>
    where TRequest : RequestBase, new()
    where TResponse : class
{
    public override ApiTypes Type { get; } = ApiTypes.Trace;
}

public abstract class TraceEndpoint<TRequest, TCommand, TResponse> : EndpointBase<TRequest, TCommand, TResponse>
    where TResponse : class
    where TRequest : RequestBase, new()
    where TCommand : IRequestBase<TResponse>, new()
{

    protected TraceEndpoint(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
    public override ApiTypes Type { get; } = ApiTypes.Trace;
}