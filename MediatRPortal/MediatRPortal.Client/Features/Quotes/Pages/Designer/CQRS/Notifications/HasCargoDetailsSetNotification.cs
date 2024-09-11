using MediatRPortal.Client.Features.Application.Components.Base;

namespace MediatRPortal.Client.Features.Quotes.Pages.Designer.CQRS.Notifications;

public record HasCargoDetailsSetNotification(Guid SessionId, bool HasDetails) : NotificationBase(SessionId);
