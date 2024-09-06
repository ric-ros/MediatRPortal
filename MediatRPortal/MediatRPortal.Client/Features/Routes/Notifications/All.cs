using MediatR;
using MediatRPortal.Client.Components.Base;
using MediatRPortal.Client.Models;

namespace MediatRPortal.Client.Features.Routes.Notifications;

public record RouteAddedNotification(Guid SessionId, RouteModel Route) : NotificationBase(SessionId);
public record RouteUpdatedNotification(Guid SessionId, RouteModel Route) : NotificationBase(SessionId);
public record RouteDeletedNotification(Guid SessionId, RouteModel Route) : NotificationBase(SessionId);
public record RoutesClearedNotification(Guid SessionId) : NotificationBase(SessionId);
