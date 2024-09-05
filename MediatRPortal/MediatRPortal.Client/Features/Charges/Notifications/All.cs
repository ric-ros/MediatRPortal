using MediatR;
using MediatRPortal.Client.Models;

namespace MediatRPortal.Client.Features.Charges.Notifications;

public record ChargeAddedNotification(ChargeModel Charge) : INotification;
public record ChargeUpdatedNotification(ChargeModel Charge) : INotification;
public record ChargeDeletedNotification(Guid ChargeId) : INotification;
public record ChargesClearedNotification : INotification;
