﻿using VisitorApp.Contract.Common;
using VisitorApp.Application.Abstractions.Messaging;

namespace VisitorApp.API.Common.Endpoints;

public abstract class OptionsEndpointWithoutRequest<TResponse> : EndpointWithoutRequestBase<TResponse>
    where TResponse : class
{
    public override ApiTypes Type { get; } = ApiTypes.Options;

}

public abstract class OptionsEndpointWithoutRequest<TRequest, TCommand, TResponse> : EndpointWithoutRequestBase<TRequest, TCommand, TResponse>
    where TResponse : class
    where TRequest : RequestBase, new()
    where TCommand : IRequestBase<TResponse>, new()
{
    protected OptionsEndpointWithoutRequest(ISender sender) : base(sender)
    {
    }

    public override ApiTypes Type { get; } = ApiTypes.Options;

}

public abstract class OptionsEndpoint<TRequest, TResponse> : EndpointBase<TRequest, TResponse>
    where TRequest : RequestBase, new()
    where TResponse : class
{
    public override ApiTypes Type { get; } = ApiTypes.Options;
}

public abstract class OptionsEndpoint<TRequest, TCommand, TResponse> : EndpointBase<TRequest, TCommand, TResponse>
    where TResponse : class
    where TRequest : RequestBase, new()
    where TCommand : IRequestBase<TResponse>, new()
{

    protected OptionsEndpoint(ISender sender, AutoMapper.IMapper mapper) : base(sender, mapper)
    {
    }
    public override ApiTypes Type { get; } = ApiTypes.Options;
}