using MediatR;
using MediatRPortal.Client.Features.Routes.Notifications;
using MediatRPortal.Client.Models;

namespace MediatRPortal.Client.Features.Routes.CommandsHandlers;

public record UpdateRouteCommand(Guid RouteId, string Origin, string Destination, string Currency) : IRequest;

public class UpdateRouteCommandHandler : IRequestHandler<UpdateRouteCommand>
{
    private readonly IMediator _mediator;

    public UpdateRouteCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(UpdateRouteCommand request, CancellationToken cancellationToken)
    {
        var fakeUpdatedRoute = new RouteModel { Id = request.RouteId, Origin = request.Origin, Destination = request.Destination };

        await _mediator.Publish(new RouteUpdatedNotification(fakeUpdatedRoute), cancellationToken);
    }
}