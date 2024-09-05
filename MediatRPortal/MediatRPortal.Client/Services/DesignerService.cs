using MediatRPortal.Client.Models;

namespace MediatRPortal.Client.Services;

public class DesignerService
{
    private bool _hasCargoDetails;
    public bool HasCargoDetails => _hasCargoDetails;

    public void SetHasCargoDetails(bool hasCargoDetails)
    {
        _hasCargoDetails = hasCargoDetails;
    }
}
