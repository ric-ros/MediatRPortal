using MediatR;
using MediatRPortal.Client.Features.Application.Components.Base;

namespace MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Routes.CQRS.Notifications;

public record RoutesClearedNotification(Guid SessionId) : NotificationBase(SessionId);
