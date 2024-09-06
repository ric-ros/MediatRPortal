using MediatR;
using MediatRPortal.Client.Components.Base;
using MediatRPortal.Client.Features.Charges.Notifications;
using MediatRPortal.Client.Features.Routes.Notifications;
using MediatRPortal.Client.Models;

namespace MediatRPortal.Client.Features.Charges.CommandsHandlers;

public record ClearChargesCommand(Guid SessionId) : RequestBase(SessionId);

public class ClearChargesCommandHandler : IRequestHandler<ClearChargesCommand>
{
    private readonly IMediator _mediator;

    public ClearChargesCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(ClearChargesCommand request, CancellationToken cancellationToken)
    {
        await _mediator.Publish(new ChargesClearedNotification(request.SessionId), cancellationToken);
    }
}