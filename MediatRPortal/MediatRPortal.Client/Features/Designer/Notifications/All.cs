using MediatR;
using MediatRPortal.Client.Components.Base;

namespace MediatRPortal.Client.Features.Designer.Notifications;

public record HasCargoDetailsSetNotification(Guid SessionId, bool HasDetails) : NotificationBase(SessionId);
