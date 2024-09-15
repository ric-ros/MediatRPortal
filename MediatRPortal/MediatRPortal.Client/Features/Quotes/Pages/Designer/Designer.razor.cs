using MediatR;
using MediatRPortal.Client.Features.Application.Components.Base;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Base;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Charges.CQRS.Notifications;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Routes.CQRS.Notifications;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.CQRS.Commands;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Models;
using Microsoft.AspNetCore.Components;

namespace MediatRPortal.Client.Features.Quotes.Pages.Designer;

public partial class Designer
{
    public Designer()
    {
        Designer = new DesignerModel();
    }

    private void SetHasDetails()
    {
        Mediator.Send(new SetCargoDetailsCommand(SessionId, Designer.HasCargoDetails));
    }

    #region Notification handlers
    protected override async Task HandleRouteAddedAsync(RouteAddedNotification notification)
    {
        await base.HandleRouteAddedAsync(notification);

        Designer.Routes.Add(notification.Route);
    }

    protected override async Task HandleRouteUpdatedAsync(RouteUpdatedNotification notification)
    {
        await base.HandleRouteUpdatedAsync(notification);

        var routeToUpdate = Designer.Routes.FirstOrDefault(x => x.Id == notification.Route.Id);

        if (routeToUpdate is not null)
        {
            routeToUpdate = notification.Route;
        }
    }

    protected override async Task HandleRouteDeletedAsync(RouteDeletedNotification notification)
    {
        await base.HandleRouteDeletedAsync(notification);

        Designer.Routes.RemoveAll(x => x.Id == notification.Route.Id);
    }

    protected override async Task HandleRoutesClearedAsync(RoutesClearedNotification notification)
    {
        await base.HandleRoutesClearedAsync(notification);

        Designer.Routes.Clear();
    }

    protected override async Task HandleChargeAddedAsync(ChargeAddedNotification notification)
    {
        await base.HandleChargeAddedAsync(notification);

        // TODO: double check, needs to go when the state is managed
        if (Designer.Routes.FirstOrDefault(x => x.Id == notification.Charge.AssociatedRouteId) is null)
        {
            return;
        }

        // TODO: double check, needs to go when the state is managed
        if (Designer.Charges.Any(x => x.Id == notification.Charge.Id))
        {
            return;
        }

        Designer.Charges.Add(notification.Charge);
    }

    protected override async Task HandleChargeUpdatedAsync(ChargeUpdatedNotification notification)
    {
        await base.HandleChargeUpdatedAsync(notification);

        var chargeToUpdate = Designer.Charges.FirstOrDefault(x => x.Id == notification.Charge.Id);

        if (chargeToUpdate is not null)
        {
            chargeToUpdate.Description = notification.Charge.Description;
        }
    }

    protected override async Task HandleChargeDeletedAsync(ChargeDeletedNotification notification)
    {
        await base.HandleChargeDeletedAsync(notification);

        Designer.Charges.RemoveAll(x => x.Id == notification.ChargeId);
    }

    protected override async Task HandleChargesClearedAsync(ChargesClearedNotification notification)
    {
        await base.HandleChargesClearedAsync(notification);

        if (notification.AssociatedRouteId is null)
        {
            Designer.Charges.Clear();
        }
        else
        {
            Designer.Charges.RemoveAll(x => x.AssociatedRouteId == notification.AssociatedRouteId);
        }
    }
    #endregion
}