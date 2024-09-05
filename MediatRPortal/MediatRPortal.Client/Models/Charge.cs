using MediatRPortal.Client.Models.Abstractions;

namespace MediatRPortal.Client.Models;

public record Charge: DesignerSection
{
    public required Guid AssociatedRouteId { get; set; }
    public string? Description { get; set; }
    public Dictionary<string, decimal?> Columns { get; set; } = [];
}
