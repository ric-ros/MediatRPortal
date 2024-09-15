using MediatR;
using MediatRPortal.Client.Features.Application.Components.Base;

namespace MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Charges.CQRS.Notifications;

/// <summary>
/// Notification that all charges have been cleared. 
/// If an associated route is provided, only charges associated with that route have been cleared.
/// </summary>
public record ChargesClearedNotification(Guid SessionId, Guid? AssociatedRouteId = null) : NotificationBase(SessionId);
