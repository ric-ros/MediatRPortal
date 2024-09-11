namespace MediatRPortal.Client.Features.Quotes.Pages.Designer.Models.Abstractions;

public abstract record DesignerSection
{
    public Guid Id { get; set; } = Guid.NewGuid();
}
