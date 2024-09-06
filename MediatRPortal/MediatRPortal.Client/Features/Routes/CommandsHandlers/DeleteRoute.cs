using MediatR;
using MediatRPortal.Client.Components.Base;
using MediatRPortal.Client.Features.Routes.Notifications;
using MediatRPortal.Client.Models;

namespace MediatRPortal.Client.Features.Routes.CommandsHandlers;

public record DeleteRouteCommand(Guid SessionId, Guid RouteId) : RequestBase(SessionId);

public class DeleteRouteCommandHandler : IRequestHandler<DeleteRouteCommand>
{
    private readonly IMediator _mediator;

    public DeleteRouteCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(DeleteRouteCommand request, CancellationToken cancellationToken)
    {
        var route = new RouteModel { Id = request.RouteId };

        await _mediator.Publish(new RouteDeletedNotification(request.SessionId, route), cancellationToken);
    }
}
