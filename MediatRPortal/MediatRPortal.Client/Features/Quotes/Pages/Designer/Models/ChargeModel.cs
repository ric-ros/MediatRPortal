using MediatRPortal.Client.Features.Quotes.Pages.Designer.Models.Abstractions;

namespace MediatRPortal.Client.Features.Quotes.Pages.Designer.Models;

public record ChargeModel : DesignerSection
{
    public required Guid AssociatedRouteId { get; set; }
    public string? Description { get; set; }
    public ChargeTypes Type { get; set; }
    public JobClasses? JobClass { get; set; }
    public Dictionary<ChargeColumn, double?> Columns { get; set; } = [];
}

[Flags] public enum ChargeTypes
{
    Origin,
    Destination,
    Freight,
    Other
}

[Flags] public enum JobClasses
{
    FCL,
    LCL,
    Air,
    Truck,
    Rail
}

public enum ChargeColumn
{
    // For all job classes
    Base,
    Minimum,

    // if FCL or Truck or Rail
    Container10,
    Container20,
    Container40,
    ContainerHC40,
    ContainerOL40,

    // if LCL
    CBM,

    // if Air
    KG,

    // additional to be used when type is Other
    Pallet,
    Carton,
    Unit
}