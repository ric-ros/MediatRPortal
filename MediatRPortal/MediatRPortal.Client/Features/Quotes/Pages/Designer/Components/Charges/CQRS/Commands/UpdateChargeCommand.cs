using MediatR;
using MediatRPortal.Client.Features.Application.Components.Base;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Charges.CQRS.Notifications;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Models;

namespace MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Charges.CQRS.Commands;

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
