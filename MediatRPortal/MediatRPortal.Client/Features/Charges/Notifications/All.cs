using MediatR;
using MediatRPortal.Client.Components.Base;
using MediatRPortal.Client.Models;

namespace MediatRPortal.Client.Features.Charges.Notifications;

public record ChargeAddedNotification(Guid SessionId, ChargeModel Charge) : NotificationBase(SessionId);
public record ChargeUpdatedNotification(Guid SessionId, ChargeModel Charge) : NotificationBase(SessionId);
public record ChargeDeletedNotification(Guid SessionId, Guid ChargeId) : NotificationBase(SessionId);
public record ChargesClearedNotification(Guid SessionId) : NotificationBase(SessionId);
