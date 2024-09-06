using MediatR;

namespace MediatRPortal.Client.Components.Base;

public record RequestBase<T>(Guid SessionId): IRequest<T>;
public record RequestBase(Guid SessionId): IRequest;
