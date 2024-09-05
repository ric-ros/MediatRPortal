using MediatR;
using MediatRPortal.Client.Features.Routes.Notifications;
using MediatRPortal.Client.Models;

namespace MediatRPortal.Client.Features.Routes.Commands;

public record AddRouteCommand(string Origin, string Destination, string Currency) : IRequest<Guid>;
public record UpdateRouteCommand(Guid RouteId, string Origin, string Destination, string Currency) : IRequest;
public record DeleteRouteCommand(Guid RouteId) : IRequest;
public record ClearRoutesCommand : IRequest;


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

public class UpdateRouteCommandHandler : IRequestHandler<UpdateRouteCommand>
{
    private readonly IMediator _mediator;

    public UpdateRouteCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(UpdateRouteCommand request, CancellationToken cancellationToken)
    {
        var fakeUpdatedRoute = new Route { Id = request.RouteId, Origin = request.Origin, Destination = request.Destination };

        await _mediator.Publish(new RouteUpdatedNotification(fakeUpdatedRoute), cancellationToken);
    }
}

public class DeleteRouteCommandHandler : IRequestHandler<DeleteRouteCommand>
{
    private readonly IMediator _mediator;

    public DeleteRouteCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(DeleteRouteCommand request, CancellationToken cancellationToken)
    {
        var route = new Route { Id = request.RouteId };

        await _mediator.Publish(new RouteDeletedNotification(route), cancellationToken);
    }
}

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