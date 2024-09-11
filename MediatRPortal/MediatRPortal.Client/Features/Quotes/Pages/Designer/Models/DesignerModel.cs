namespace MediatRPortal.Client.Features.Quotes.Pages.Designer.Models;

public record DesignerModel
{
    public bool HasCargoDetails { get; set; }
    public List<ChargeModel> Charges { get; set; } = [];
    public List<RouteModel> Routes { get; set; } = [];
}
