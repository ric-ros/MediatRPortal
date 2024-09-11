using MediatR;
using MediatRPortal.Client.Features.Application.Components.Base;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Charges.CQRS.Notifications;

namespace MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Charges.CQRS.Commands;

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