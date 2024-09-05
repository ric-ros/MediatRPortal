using MediatR;
using MediatRPortal.Client.Features.Charges.Notifications;
using MediatRPortal.Client.Features.Routes.Notifications;
using MediatRPortal.Client.Models;

namespace MediatRPortal.Client.Features.Routes.CommandsHandlers;

public record ClearRoutesCommand : IRequest;

public class ClearRoutesCommandHandler : IRequestHandler<ClearRoutesCommand>
{
    private readonly IMediator _mediator;

    public ClearRoutesCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(ClearRoutesCommand request, CancellationToken cancellationToken)
    {
        await _mediator.Publish(new RoutesClearedNotification(), cancellationToken);
    }
}