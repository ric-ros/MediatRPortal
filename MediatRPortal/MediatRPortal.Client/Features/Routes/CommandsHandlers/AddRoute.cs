using MediatR;
using MediatRPortal.Client.Features.Routes.Notifications;
using MediatRPortal.Client.Models;

namespace MediatRPortal.Client.Features.Routes.CommandsHandlers;

public record AddRouteCommand(string Origin, string Destination, string Currency) : IRequest<Guid>;

public class AddRouteCommandHandler : IRequestHandler<AddRouteCommand, Guid>
{
    private readonly IMediator _mediator;

    public AddRouteCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Guid> Handle(AddRouteCommand request, CancellationToken cancellationToken)
    {
        var route = new RouteModel { Origin = request.Origin, Destination = request.Destination };
        await _mediator.Publish(new RouteAddedNotification(route), cancellationToken);
        return route.Id;
    }
}