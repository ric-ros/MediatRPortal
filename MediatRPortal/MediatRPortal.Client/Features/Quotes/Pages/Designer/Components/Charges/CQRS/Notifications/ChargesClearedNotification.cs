using MediatR;
using MediatRPortal.Client.Features.Application.Components.Base;

namespace MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Charges.CQRS.Notifications;

public record ChargesClearedNotification(Guid SessionId) : NotificationBase(SessionId);
