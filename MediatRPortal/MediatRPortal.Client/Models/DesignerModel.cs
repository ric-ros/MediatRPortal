namespace MediatRPortal.Client.Models;

public record DesignerModel
{
    public bool HasCargoDetails { get; set; }
    public List<ChargeModel> Charges { get; set; } = [];
    public List<RouteModel> Routes { get; set; } = [];
}
