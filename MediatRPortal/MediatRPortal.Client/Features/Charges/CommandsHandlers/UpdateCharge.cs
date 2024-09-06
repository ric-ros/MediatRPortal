using MediatR;
using MediatRPortal.Client.Components.Base;
using MediatRPortal.Client.Features.Charges.Notifications;
using MediatRPortal.Client.Models;

namespace MediatRPortal.Client.Features.Charges.CommandsHandlers;

public record UpdateChargeCommand(Guid SessionId, Guid AssociatedRouteId, Guid ChargeId, string Description, Dictionary<string, decimal?> Columns) : RequestBase(SessionId);

public class UpdateChargeCommandHandler : IRequestHandler<UpdateChargeCommand>
{
    private readonly IMediator _mediator;

    public UpdateChargeCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(UpdateChargeCommand request, CancellationToken cancellationToken)
    {
        var charge = new ChargeModel
        {
            Id = request.ChargeId,
            AssociatedRouteId = request.AssociatedRouteId,
            Description = request.Description,
            Columns = request.Columns
        };

        await _mediator.Publish(new ChargeUpdatedNotification(request.SessionId, charge), cancellationToken);
    }
}
