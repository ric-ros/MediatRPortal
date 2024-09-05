using MediatR;
using MediatRPortal.Client.Features.Charges.Notifications;
using MediatRPortal.Client.Models;

namespace MediatRPortal.Client.Features.Charges.CommandsHandlers;

public record UpdateChargeCommand(Guid AssociatedRouteId, Guid ChargeId, string Description, Dictionary<string, decimal?> Columns) : IRequest;

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

        await _mediator.Publish(new ChargeUpdatedNotification(charge), cancellationToken);
    }
}
