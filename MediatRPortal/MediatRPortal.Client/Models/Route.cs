using MediatRPortal.Client.Models.Abstractions;

namespace MediatRPortal.Client.Models;

public record Route : DesignerSection
{
    public string? Origin { get; set; }
    public string? Destination { get; set; }
    public string? Currency { get; set; }
}
