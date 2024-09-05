using MediatR;
using MediatRPortal.Client.Features.Charges.Notifications;
using MediatRPortal.Client.Models;

namespace MediatRPortal.Client.Features.Charges.CommandsHandlers;

public record AddChargeCommand(Guid AssociatedRouteId, string? Description, Dictionary<string, decimal?> Columns) : IRequest<Guid>;

public class AddChargeCommandHandler : IRequestHandler<AddChargeCommand, Guid>
{
    private readonly IMediator _mediator;

    public AddChargeCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Guid> Handle(AddChargeCommand request, CancellationToken cancellationToken)
    {
        var charge = new ChargeModel
        {
            AssociatedRouteId = request.AssociatedRouteId,
            Description = request.Description,
            Columns = request.Columns
        };
        await _mediator.Publish(new ChargeAddedNotification(charge), cancellationToken);
        return charge.Id;
    }
}
