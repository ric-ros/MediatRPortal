using MediatRPortal.Client.Features.Quotes.Pages.Designer.Models.Abstractions;

namespace MediatRPortal.Client.Features.Quotes.Pages.Designer.Models;

public record RouteModel : DesignerSection
{
    public string? Origin { get; set; }
    public string? Destination { get; set; }
    public string? Currency { get; set; }
}
