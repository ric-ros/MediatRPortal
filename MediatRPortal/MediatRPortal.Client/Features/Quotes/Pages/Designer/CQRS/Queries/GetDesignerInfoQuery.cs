using MediatR;
using MediatRPortal.Client.Features.Application.Components.Base;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Models;

namespace MediatRPortal.Client.Features.Quotes.Pages.Designer.CQRS.Queries;

public record GetDesignerInfoQuery(Guid SessionId) : RequestBase<DesignerModel>(SessionId);
