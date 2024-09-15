using MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Charges;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Charges.CQRS.Commands;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Charges.CQRS.Notifications;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Routes.CQRS.Commands;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Routes.CQRS.Notifications;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.CQRS.Commands;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Models;

namespace MediatRPortal.Client.Features.Quotes.Pages.Designer;

public partial class PrecompiledDesigner
{
    public PrecompiledDesigner()
    {
        Designer = new DesignerModel();
    }

    protected override async Task OnInitializedAsync()
    {

        static List<ChargeModel> MockChargeData(RouteModel route)
        {
            var charges = new List<ChargeModel>();
            var random = new Random();

            foreach (ChargeTypes chargeType in Enum.GetValues(typeof(ChargeTypes)))
            {
                if (route.ChargeTypes.HasFlag(chargeType))
                {
                    var charge = new ChargeModel
                    {
                        AssociatedRouteId = route.Id,
                        Type = chargeType,
                        Description = $"Mock {chargeType} Charge",
                        Columns = []
                    };

                    // Add base columns for all charge types
                    charge.Columns[ChargeColumn.Base] = Math.Round(random.Next(100, 1000) + random.NextDouble(), 2);
                    charge.Columns[ChargeColumn.Minimum] = Math.Round(random.Next(50, 500) + random.NextDouble(), 2);

                    // Add additional columns based on job classes
                    if (route.JobClasses.HasFlag(JobClasses.FCL) ||
                        route.JobClasses.HasFlag(JobClasses.Truck) ||
                        route.JobClasses.HasFlag(JobClasses.Rail))
                    {
                        charge.Columns[ChargeColumn.Container10] = Math.Round(random.Next(200, 2000) + random.NextDouble(), 2);
                        charge.Columns[ChargeColumn.Container20] = Math.Round(random.Next(400, 4000) + random.NextDouble(), 2);
                        charge.Columns[ChargeColumn.Container40] = Math.Round(random.Next(800, 8000) + random.NextDouble(), 2);
                        charge.Columns[ChargeColumn.ContainerHC40] = Math.Round(random.Next(1000, 10000) + random.NextDouble(), 2);
                        charge.Columns[ChargeColumn.ContainerOL40] = Math.Round(random.Next(1200, 12000) + random.NextDouble(), 2);
                    }

                    if (route.JobClasses.HasFlag(JobClasses.LCL))
                    {
                        charge.Columns[ChargeColumn.CBM] = Math.Round(random.Next(10, 100) + random.NextDouble(), 2);
                    }

                    if (route.JobClasses.HasFlag(JobClasses.Air))
                    {
                        charge.Columns[ChargeColumn.KG] = Math.Round(random.Next(1, 10) + random.NextDouble(), 2);
                    }

                    // Add additional columns for other charge types
                    if (chargeType == ChargeTypes.Other)
                    {
                        charge.Columns[ChargeColumn.Pallet] = Math.Round(random.Next(50, 500) + random.NextDouble(), 2);
                        charge.Columns[ChargeColumn.Carton] = Math.Round(random.Next(10, 100) + random.NextDouble(), 2);
                        charge.Columns[ChargeColumn.Unit] = Math.Round(random.Next(5, 50) + random.NextDouble(), 2);
                    }

                    charges.Add(charge);
                }
            }

            return charges;
        }
        async Task FillInitialChargesInRoute(RouteModel route)
        {
            var charges = MockChargeData(route);
            // notify the charges were added
            foreach (var charge in charges)
            {
                await Mediator.Publish(new ChargeAddedNotification(SessionId, charge));
            }
        }

        await base.OnInitializedAsync();

        var routes = await Mediator.Send(new GenerateRoutesCommand(SessionId, 3));
        // mock an initial data
        foreach (var route in routes)
        {
            await FillInitialChargesInRoute(route);
        }
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