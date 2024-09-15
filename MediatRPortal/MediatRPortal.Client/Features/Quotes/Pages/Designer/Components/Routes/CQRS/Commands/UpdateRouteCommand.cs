using MediatR;
using MediatRPortal.Client.Features.Application.Components.Base;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Routes.CQRS.Notifications;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Models;

namespace MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Routes.CQRS.Commands;

public record UpdateRouteCommand(Guid SessionId, RouteModel Route) : RequestBase(SessionId);

public class UpdateRouteCommandHandler : IRequestHandler<UpdateRouteCommand>
{
    private readonly IMediator _mediator;

    public UpdateRouteCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(UpdateRouteCommand request, CancellationToken cancellationToken)
    {
        var updatedRoute = request.Route with { };

        await _mediator.Publish(new RouteUpdatedNotification(request.SessionId, updatedRoute), cancellationToken);
    }
}