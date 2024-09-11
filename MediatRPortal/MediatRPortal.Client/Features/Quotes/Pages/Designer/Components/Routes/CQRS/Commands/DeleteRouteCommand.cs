using MediatR;
using MediatRPortal.Client.Features.Application.Components.Base;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Routes.CQRS.Notifications;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Models;

namespace MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Routes.CQRS.Commands;

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
