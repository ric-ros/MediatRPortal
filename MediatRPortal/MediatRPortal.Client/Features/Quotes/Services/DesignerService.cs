namespace MediatRPortal.Client.Features.Quotes.Services;

public class DesignerService
{
    private bool _hasCargoDetails;
    public bool HasCargoDetails => _hasCargoDetails;

    public void SetHasCargoDetails(bool hasCargoDetails)
    {
        _hasCargoDetails = hasCargoDetails;
    }
}
