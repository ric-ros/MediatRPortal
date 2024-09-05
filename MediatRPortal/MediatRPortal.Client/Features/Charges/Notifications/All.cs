using MediatR;
using MediatRPortal.Client.Models;

namespace MediatRPortal.Client.Features.Charges.Notifications;

public record ChargeAddedNotification(Charge Charge) : INotification;
public record ChargeUpdatedNotification(Charge Charge) : INotification;
