namespace MediatRPortal.Client.Models.Abstractions;

public abstract record DesignerSection
{
    public Guid Id { get; set; } = Guid.NewGuid();
}
