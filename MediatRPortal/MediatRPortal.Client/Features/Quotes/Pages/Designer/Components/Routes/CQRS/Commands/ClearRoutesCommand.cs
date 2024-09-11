using MediatR;
using MediatRPortal.Client.Features.Application.Components.Base;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Routes.CQRS.Notifications;

namespace MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Routes.CQRS.Commands;

public record ClearRoutesCommand(Guid SessionId) : RequestBase(SessionId);

public class ClearRoutesCommandHandler : IRequestHandler<ClearRoutesCommand>
{
    private readonly IMediator _mediator;

    public ClearRoutesCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(ClearRoutesCommand request, CancellationToken cancellationToken)
    {
        await _mediator.Publish(new RoutesClearedNotification(request.SessionId), cancellationToken);
    }
}