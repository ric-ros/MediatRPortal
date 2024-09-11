using MediatR;

namespace MediatRPortal.Client.Features.Application.Components.Base;

public record RequestBase<T>(Guid SessionId) : IRequest<T>;
public record RequestBase(Guid SessionId) : IRequest;
