using MediatR;
using MediatRPortal.Client.Components.Base;
using MediatRPortal.Client.Features.Charges.Notifications;
using MediatRPortal.Client.Models;

namespace MediatRPortal.Client.Features.Charges.CommandsHandlers;

public record DeleteChargeCommand(Guid SessionId, Guid ChargeId) : RequestBase(SessionId);

public class DeleteChargeCommandHandler : IRequestHandler<DeleteChargeCommand>
{
    private readonly IMediator _mediator;

    public DeleteChargeCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(DeleteChargeCommand request, CancellationToken cancellationToken)
    {
        await _mediator.Publish(new ChargeDeletedNotification(request.SessionId, request.ChargeId), cancellationToken);
    }
}
