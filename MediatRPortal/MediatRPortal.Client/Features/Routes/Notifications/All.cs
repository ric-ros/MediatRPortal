using MediatR;
using MediatRPortal.Client.Models;

namespace MediatRPortal.Client.Features.Routes.Notifications;

public record RouteAddedNotification(Route Route) : INotification;
public record RouteUpdatedNotification(Route Route) : INotification;
public record RouteDeletedNotification(Route Route) : INotification;
public record RoutesClearedNotification : INotification;
