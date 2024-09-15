using MediatR;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Charges.Components;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Charges.CQRS.Commands;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Charges.CQRS.Notifications;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Routes.CQRS.Notifications;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Charges;

public partial class ChargeTabs
{
    private readonly List<RouteModel> _routes = [];
    private readonly Dictionary<Guid, List<ChargeModel>> _chargesByRoute = [];

    private IEnumerable<ChargeColumn> _selectedColumns = [ChargeColumn.Base, ChargeColumn.Minimum, ChargeColumn.Container20, ChargeColumn.Container40, ChargeColumn.ContainerHC40];
    private readonly List<string> _allChargeDescriptions =
    [
        "Ocean Freight", "Documentation", "Customs Clearance", "Insurance", "Handling",
        "Port Charges", "Fuel Surcharge", "Security Surcharge", "Terminal Handling",
        "Container Cleaning", "Container Inspection", "Container Repair", "Container Storage"
    ];

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Designer?.Routes is not null)
        {
            _routes.AddRange(Designer.Routes);
        }

        if (Designer?.Charges is not null)
        {
            foreach (var charge in Designer.Charges)
            {
                if (_chargesByRoute.TryGetValue(charge.AssociatedRouteId, out var charges))
                {
                    charges.Add(charge);
                }
                else
                {
                    _chargesByRoute.Add(charge.AssociatedRouteId, [charge]);
                }
            }
        }
    }

    public void HandleSelectedColumnChanged()
    {
        foreach (var route in _routes)
        {
            if (!_chargesByRoute.TryGetValue(route.Id, out var charges))
            {
                continue;
            }

            _chargesByRoute[route.Id] = charges.Select(c =>
            {
                var newCharge = c with { }; // clone the charge

                // remove any columns that are not in the selected columns
                foreach (var column in c.Columns.Keys)
                {
                    if (!_selectedColumns.Contains(column))
                    {
                        newCharge.Columns.Remove(column);
                    }
                }

                // add any columns that are in the selected columns but not in the charge
                foreach (var column in _selectedColumns)
                {
                    if (!newCharge.Columns.ContainsKey(column))
                    {
                        newCharge.Columns.Add(column, null);
                    }
                }

                return newCharge;
            }).ToList();
        }

        StateHasChanged();
    }
    #region MediatR Notifications

    protected override async Task HandleRouteAddedAsync(RouteAddedNotification notification)
    {
        await base.HandleRouteAddedAsync(notification);

        if (!_routes.Any(r => r.Id == notification.Route.Id))
        {
            _routes.Add(notification.Route);
        }

        // if any charge for route, do not add 
        if (!Designer.Charges.Any(c => c.AssociatedRouteId == notification.Route.Id))
        {
            await FillInitialChargesInRoute(notification.Route);
        }

        List<ChargeModel> MockChargeData(RouteModel route)
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
                        Description = _allChargeDescriptions[random.Next(0, _allChargeDescriptions.Count)],
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
    }

    protected override async Task HandleRouteUpdatedAsync(RouteUpdatedNotification notification)
    {
        await base.HandleRouteUpdatedAsync(notification);

        var route = _routes.FirstOrDefault(r => r.Id == notification.Route.Id);
        if (route is null)
        {
            return;
        }

        var charges = _chargesByRoute[route.Id];
        route.Origin = notification.Route.Origin;
        route.Destination = notification.Route.Destination;
        route.Currency = notification.Route.Currency;
        charges.ForEach(charge => Mediator.Publish(new ChargeUpdatedNotification(SessionId, charge)));
    }

    protected override async Task HandleRouteDeletedAsync(RouteDeletedNotification notification)
    {
        await base.HandleRouteDeletedAsync(notification);

        var route = _routes.FirstOrDefault(r => r.Id == notification.Route.Id);
        if (route is null)
        {
            return;
        }

        // delete all charges associated with the route and remove the route
        await Mediator.Publish(new ChargesClearedNotification(SessionId, route.Id));

        _routes.Remove(route);
        _chargesByRoute.Remove(route.Id);
    }

    protected override async Task HandleRoutesClearedAsync(RoutesClearedNotification notification)
    {
        await base.HandleRoutesClearedAsync(notification);

        _routes.Clear();
        _chargesByRoute.Clear();

        await Mediator.Publish(new ChargesClearedNotification(SessionId));
    }

    protected override async Task HandleChargeAddedAsync(ChargeAddedNotification notification)
    {
        await base.HandleChargeAddedAsync(notification);

        var route = _routes.FirstOrDefault(r => r.Id == notification.Charge.AssociatedRouteId);
        if (route is not null)
        {
            if (_chargesByRoute.TryGetValue(route.Id, out var charges))
            {
                if (!charges.Any(c => c.Id == notification.Charge.Id))
                {
                    charges.Add(notification.Charge);
                }
            }
            else
            {
                _chargesByRoute.Add(route.Id, [notification.Charge]);
            }
        }
    }

    protected override async Task HandleChargeUpdatedAsync(ChargeUpdatedNotification notification)
    {
        await base.HandleChargeUpdatedAsync(notification);

        var route = _routes.FirstOrDefault(r => r.Id == notification.Charge.AssociatedRouteId);
        if (route is null || !_chargesByRoute.TryGetValue(route.Id, out var charges))
        {
            return;
        }

        var charge = charges.FirstOrDefault(c => c.Id == notification.Charge.Id);
        if (charge is null)
        {
            return;
        }

        charge.Description = notification.Charge.Description;
        charge.Columns = notification.Charge.Columns;
    }

    protected override async Task HandleChargeDeletedAsync(ChargeDeletedNotification notification)
    {
        await base.HandleChargeDeletedAsync(notification);

        var charge = _chargesByRoute.FirstOrDefault(c => c.Value.Any(c => c.Id == notification.ChargeId))
                                    .Value
                                    .FirstOrDefault(c => c.Id == notification.ChargeId);
        if (charge is null)
        {
            return;
        }

        var route = _routes.FirstOrDefault(r => r.Id == charge.AssociatedRouteId);
        if (route is null)
        {
            return;
        }

        _chargesByRoute[route.Id].Remove(charge);
    }

    protected override async Task HandleChargesClearedAsync(ChargesClearedNotification notification)
    {
        await base.HandleChargesClearedAsync(notification);

        if (notification.AssociatedRouteId is not null)
        {
            _chargesByRoute.Remove(notification.AssociatedRouteId.Value);
        }
        else
        {
            _chargesByRoute.Clear();
        }
    }
    #endregion
}