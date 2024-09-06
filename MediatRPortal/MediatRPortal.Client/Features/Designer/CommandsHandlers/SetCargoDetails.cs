using MediatR;
using MediatRPortal.Client.Components.Base;
using MediatRPortal.Client.Features.Designer.Notifications;

namespace MediatRPortal.Client.Features.Designer.CommandsHandlers;

public record SetCargoDetailsCommand(Guid SessionId, bool HasCargoDetails) : RequestBase(SessionId);

public class SetCargoDetailsCommandHandler : IRequestHandler<SetCargoDetailsCommand>
{
    private readonly IMediator _mediator;
    public SetCargoDetailsCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task Handle(SetCargoDetailsCommand request, CancellationToken cancellationToken)
    {
        return _mediator.Publish(new HasCargoDetailsSetNotification(request.SessionId, request.HasCargoDetails), cancellationToken);
    }
}
