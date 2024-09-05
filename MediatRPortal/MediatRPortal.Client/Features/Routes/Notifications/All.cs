using MediatR;
using MediatRPortal.Client.Models;

namespace MediatRPortal.Client.Features.Routes.Notifications;

public record RouteAddedNotification(RouteModel Route) : INotification;
public record RouteUpdatedNotification(RouteModel Route) : INotification;
public record RouteDeletedNotification(RouteModel Route) : INotification;
public record RoutesClearedNotification : INotification;
