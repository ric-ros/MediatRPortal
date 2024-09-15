using MediatR;
using MediatRPortal.Client.Features.Application.Components.Base;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Charges.CQRS.Notifications;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Models;

namespace MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Charges.CQRS.Commands;

public record UpdateChargeCommand(Guid SessionId, ChargeModel Charge) : RequestBase(SessionId);

public class UpdateChargeCommandHandler : IRequestHandler<UpdateChargeCommand>
{
    private readonly IMediator _mediator;

    public UpdateChargeCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(UpdateChargeCommand request, CancellationToken cancellationToken)
    {
        var charge = request.Charge with { };

        await _mediator.Publish(new ChargeUpdatedNotification(request.SessionId, charge), cancellationToken);
    }
}
