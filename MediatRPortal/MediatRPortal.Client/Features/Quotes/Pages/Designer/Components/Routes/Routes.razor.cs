using MediatR;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Routes.CQRS.Commands;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Routes.CQRS.Notifications;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Models;
using Microsoft.AspNetCore.Components;

namespace MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Routes;

public partial class Routes
{
    private List<RouteModel> _routes = [];

    private int _routesCount = 1;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Designer?.Routes is not null)
        {
            _routes = [.. Designer.Routes];
        }
    }

    private async Task AddRoute()
    {
        await Mediator.Send(new AddRouteCommand(SessionId));
    }

    private async Task GenerateRoutes()
    {
        await Mediator.Send(new GenerateRoutesCommand(SessionId, _routesCount));
    }

    private async Task ClearRoutes()
    {
        await Mediator.Send(new ClearRoutesCommand(SessionId));
    }

    protected override async Task HandleRouteAddedAsync(RouteAddedNotification notification)
    {
        await base.HandleRouteAddedAsync(notification);

        _routes.Add(notification.Route);
    }

    protected override async Task HandleRouteUpdatedAsync(RouteUpdatedNotification notification)
    {
        await base.HandleRouteUpdatedAsync(notification);

        var routeToUpdate = _routes.FirstOrDefault(x => x.Id == notification.Route.Id);
        if (routeToUpdate is not null)
        {
            routeToUpdate = notification.Route;
        }
    }

    protected override async Task HandleRouteDeletedAsync(RouteDeletedNotification notification)
    {
        await base.HandleRouteDeletedAsync(notification);

        _routes.RemoveAll(x => x.Id == notification.Route.Id);
    }

    protected override async Task HandleRoutesClearedAsync(RoutesClearedNotification notification)
    {
        await base.HandleRoutesClearedAsync(notification);

        _routes.Clear();
    }
}