using MediatRPortal.Client.Features.Application.Components.Base;

namespace MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Charges.CQRS.Notifications;

public record ChargeDeletedNotification(Guid SessionId, Guid ChargeId) : NotificationBase(SessionId);
