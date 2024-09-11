using MediatRPortal.Client.Features.Application.Components.Base;
using MediatRPortal.Client.Features.Quotes.Pages.Designer.Models;

namespace MediatRPortal.Client.Features.Quotes.Pages.Designer.Components.Charges.CQRS.Notifications;

public record ChargeAddedNotification(Guid SessionId, ChargeModel Charge) : NotificationBase(SessionId);
