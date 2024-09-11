using MediatRPortal.Client.Features.Application.Components.Base;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Models;

namespace MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Routes.CQRS.Notifications;

public record RouteDeletedNotification(Guid SessionId, RouteModel Route) : NotificationBase(SessionId);
