using MediatR;
using MediatRPortal.Client.Features.Application.Components.Base;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Charges.CQRS.Notifications;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Routes.CQRS.Notifications;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.CQRS.Notifications;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Models;
using Microsoft.AspNetCore.Components;

namespace MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Base;

public abstract class DesignerComponentBase : NotificationComponentBase<NotificationBase>
{
    [Parameter, Obsolete("Remove and retrieve it from the state manager")]
    public DesignerModel? Designer { get; set; }

    protected override void RegisterNotificationHandlers()
    {
        base.RegisterNotificationHandlers();

        RegisterNotificationHandler<HasCargoDetailsSetNotification>(HandleCargoDetailsSetAsync, _ => UpdateStateAsync());

        RegisterNotificationHandler<RouteAddedNotification>(HandleRouteAddedAsync, _ => UpdateStateAsync());
        RegisterNotificationHandler<RouteUpdatedNotification>(HandleRouteUpdatedAsync, _ => UpdateStateAsync());
        RegisterNotificationHandler<RouteDeletedNotification>(HandleRouteDeletedAsync, _ => UpdateStateAsync());
        RegisterNotificationHandler<RoutesClearedNotification>(HandleRoutesClearedAsync, _ => UpdateStateAsync());

        RegisterNotificationHandler<ChargeAddedNotification>(HandleChargeAddedAsync, _ => UpdateStateAsync());
        RegisterNotificationHandler<ChargeUpdatedNotification>(HandleChargeUpdatedAsync, _ => UpdateStateAsync());
        RegisterNotificationHandler<ChargeDeletedNotification>(HandleChargeDeletedAsync, _ => UpdateStateAsync());
        RegisterNotificationHandler<ChargesClearedNotification>(HandleChargesClearedAsync, _ => UpdateStateAsync());
    }

    private Task UpdateStateAsync() => InvokeAsync(StateHasChanged);

    protected virtual Task HandleCargoDetailsSetAsync(HasCargoDetailsSetNotification notification)
    {
        if (Designer is null)
        {
            return Task.CompletedTask;
        }

        Designer.HasCargoDetails = notification.HasDetails;

        return Task.CompletedTask;
    }

    protected virtual Task HandleRouteAddedAsync(RouteAddedNotification notification) => Task.CompletedTask;
    protected virtual Task HandleRouteUpdatedAsync(RouteUpdatedNotification notification) => Task.CompletedTask;
    protected virtual Task HandleRouteDeletedAsync(RouteDeletedNotification notification) => Task.CompletedTask;
    protected virtual Task HandleRoutesClearedAsync(RoutesClearedNotification notification) => Task.CompletedTask;

    protected virtual Task HandleChargeAddedAsync(ChargeAddedNotification notification) => Task.CompletedTask;
    protected virtual Task HandleChargeUpdatedAsync(ChargeUpdatedNotification notification) => Task.CompletedTask;
    protected virtual Task HandleChargeDeletedAsync(ChargeDeletedNotification notification) => Task.CompletedTask;
    protected virtual Task HandleChargesClearedAsync(ChargesClearedNotification notification) => Task.CompletedTask;
}
