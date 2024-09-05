using MediatRPortal.Client.Models.Abstractions;

namespace MediatRPortal.Client.Models;

public class Route : DesignerSection
{
    public string? Origin { get; set; }
    public string? Destination { get; set; }
    public string? Currency { get; set; }
}
