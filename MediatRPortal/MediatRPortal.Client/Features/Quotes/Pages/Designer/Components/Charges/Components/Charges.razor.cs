using MediatR;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Charges.Components;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Charges.CQRS.Commands;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Charges.CQRS.Notifications;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Routes.CQRS.Notifications;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Charges.Components;

public partial class Charges
{
    [Parameter]
    public List<RouteModel>? Routes { get; set; }

    [Parameter]
    public Dictionary<Guid, List<ChargeModel>>? ChargesByRoute { get; set; }

    private IEnumerable<ChargeColumn> _selectedColumns = [ChargeColumn.Base, ChargeColumn.Minimum, ChargeColumn.Container20, ChargeColumn.Container40, ChargeColumn.ContainerHC40];

    [Parameter]
    public IEnumerable<ChargeColumn>? SelectedColumns { get; set; }

    [Parameter]
    public EventCallback<IEnumerable<ChargeColumn>?> SelectedColumnsChanged { get; set; }

    [Parameter]
    public bool HasCargoDetails { get; set; }


    private readonly List<ChargeColumn> _allColumns =
    [
        ChargeColumn.Base,
        ChargeColumn.Minimum,
        // if FCL or Truck or Rail
        ChargeColumn.Container10,
        ChargeColumn.Container20,
        ChargeColumn.Container40,
        ChargeColumn.ContainerHC40,
        ChargeColumn.ContainerOL40,
        // if LCL
        ChargeColumn.CBM,
        // if Air
        ChargeColumn.KG,
        // additional to be used when type 
        ChargeColumn.Pallet,
        ChargeColumn.Carton,
        ChargeColumn.Unit
    ];


    private readonly List<string> _allChargeDescriptions =
    [
        "Ocean Freight", "Documentation", "Customs Clearance", "Insurance", "Handling",
        "Port Charges", "Fuel Surcharge", "Security Surcharge", "Terminal Handling",
        "Container Cleaning", "Container Inspection", "Container Repair", "Container Storage"
    ];

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }

    public void HandleSelectedColumnChanged(IEnumerable<ChargeColumn>? selectedColumns)
    {
        _selectedColumns = selectedColumns ?? [];
        SelectedColumnsChanged.InvokeAsync(_selectedColumns);
    }

    private async Task AddCharge(Guid routeId, string value)
    {
        var route = Routes?.FirstOrDefault(r => r.Id == routeId);
        if (route is not null)
        {
            var newCharge = new ChargeModel
            {
                AssociatedRouteId = routeId,
                Description = value,
                Columns = SelectedColumns.ToDictionary(c => c, _ => (double?)null)
            };

            var chargeId = await Mediator.Send(
                new AddChargeCommand(SessionId, newCharge));

            value = "";

            // var parameters = new DialogParameters<ChargeDialog> { { x => x.Charge, charge }, { x => x.Route, route } };

            // _ = await DialogService.ShowAsync<ChargeDialog>("Charge", parameters);
        }
    }

    private async Task DeleteCharge(Guid routeId, Guid chargeId)
    {
        var route = Routes?.FirstOrDefault(r => r.Id == routeId);
        if (route is null || ChargesByRoute is null || !ChargesByRoute.TryGetValue(routeId, out List<ChargeModel>? value))
        {
            return;
        }

        var charge = value.FirstOrDefault(c => c.Id == chargeId);
        if (charge is null)
        {
            return;
        }

        var result = await JsRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete this charge?");
        if (result)
        {
            await Mediator.Send(new DeleteChargeCommand(SessionId, chargeId));
        }
    }

    private async Task HandleRowClick(ChargeModel charge, RouteModel route)
    {
        // open dialog to change charge values
        var parameters = new DialogParameters<ChargeDialog> { { x => x.Charge, charge }, { x => x.Route, route } };

        _ = await DialogService.ShowAsync<ChargeDialog>("Charge", parameters);
        // var result = await dialog.Result;

        // if (result?.Data is Charge modifiedCharge)
        // {
        //     // here we would do the normal update logic but we will handle it with mediator inside the dialog
        // }
    }

    private static string GetChargeValue(ChargeModel charge, ChargeColumn column)
    {
        return charge.Columns.TryGetValue(column, out var value)
            ? value?.ToString() ?? string.Empty
            : string.Empty;
    }

}