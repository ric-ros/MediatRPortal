using MediatR;
using MediatRPortal.Client.Components.Base;
using MediatRPortal.Client.Features.Charges.Notifications;
using MediatRPortal.Client.Models;

namespace MediatRPortal.Client.Features.Charges.CommandsHandlers;

public record AddChargeCommand(Guid SessionId, Guid AssociatedRouteId, string? Description, Dictionary<string, decimal?> Columns) : RequestBase<Guid>(SessionId);

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
        await _mediator.Publish(new ChargeAddedNotification(request.SessionId, charge), cancellationToken);
        return charge.Id;
    }
}
