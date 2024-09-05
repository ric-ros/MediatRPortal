using MediatR;
using MediatRPortal.Client.Features.Charges.Notifications;
using MediatRPortal.Client.Models;

namespace MediatRPortal.Client.Features.Charges.Commands;

public record AddChargeCommand(Guid AssociatedRouteId, string Description, Dictionary<string, decimal?> Columns) : IRequest<Guid>;
public record UpdateChargeCommand(Guid AssociatedRouteId, Guid ChargeId, string Description, Dictionary<string, decimal?> Columns) : IRequest;


public class AddChargeCommandHandler : IRequestHandler<AddChargeCommand, Guid>
{
    private readonly IMediator _mediator;

    public AddChargeCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Guid> Handle(AddChargeCommand request, CancellationToken cancellationToken)
    {
        var charge = new Charge
        {
            AssociatedRouteId = request.AssociatedRouteId,
            Description = request.Description,
            Columns = request.Columns
        };
        await _mediator.Publish(new ChargeAddedNotification(charge), cancellationToken);
        return charge.Id;
    }
}

public class UpdateChargeCommandHandler : IRequestHandler<UpdateChargeCommand>
{
    private readonly IMediator _mediator;

    public UpdateChargeCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(UpdateChargeCommand request, CancellationToken cancellationToken)
    {
        var charge = new Charge
        {
            Id = request.ChargeId,
            AssociatedRouteId = request.AssociatedRouteId,
            Description = request.Description,
            Columns = request.Columns
        };

        await _mediator.Publish(new ChargeUpdatedNotification(charge), cancellationToken);
    }
}
