using MediatRPortal.Client.Features.Quotes.Pages.Designer.Models.Abstractions;

namespace MediatRPortal.Client.Features.Quotes.Pages.Designer.Models;

public record ChargeModel : DesignerSection
{
    public required Guid AssociatedRouteId { get; set; }
    public string? Description { get; set; }
    public Dictionary<string, decimal?> Columns { get; set; } = [];
}
