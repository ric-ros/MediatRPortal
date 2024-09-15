using MediatR;
using MediatRPortal.Client.Features.Application.Components.Base;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Routes.CQRS.Notifications;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Models;

namespace MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Routes.CQRS.Commands;

public record AddRouteCommand(Guid SessionId) : RequestBase<Guid>(SessionId);

public class AddRouteCommandHandler : IRequestHandler<AddRouteCommand, Guid>
{
    private readonly IMediator _mediator;

    public AddRouteCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Guid> Handle(AddRouteCommand request, CancellationToken cancellationToken)
    {
        var route = new RouteModel();
        await _mediator.Publish(new RouteAddedNotification(request.SessionId, route), cancellationToken);
        return route.Id;
    }
}