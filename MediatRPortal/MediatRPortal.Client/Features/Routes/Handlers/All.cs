using MediatR;
using MediatRPortal.Client.Features.Charges.Commands;
using MediatRPortal.Client.Features.Routes.Commands;
using MediatRPortal.Client.Features.Routes.Notifications;
using MediatRPortal.Client.Models;
using MediatRPortal.Client.Services;

namespace MediatRPortal.Client.Features.Routes.Handlers;

public class AddRouteCommandHandler : IRequestHandler<AddRouteCommand, Guid>
{
    private readonly IMediator _mediator;

    public AddRouteCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Guid> Handle(AddRouteCommand request, CancellationToken cancellationToken)
    {
        var route = new Route { Origin = request.Origin, Destination = request.Destination };
        await _mediator.Publish(new RouteAddedNotification(route), cancellationToken);
        return route.Id;
    }
}