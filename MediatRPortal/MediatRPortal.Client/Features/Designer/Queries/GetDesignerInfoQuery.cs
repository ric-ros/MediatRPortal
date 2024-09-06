using MediatR;
using MediatRPortal.Client.Components.Base;

namespace MediatRPortal.Client.Models;

public record GetDesignerInfoQuery(Guid SessionId) : RequestBase<DesignerModel>(SessionId);
