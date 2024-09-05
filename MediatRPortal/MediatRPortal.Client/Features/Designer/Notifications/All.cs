using MediatR;

namespace MediatRPortal.Client.Features.Designer.Notifications;

public record HasCargoDetailsSetNotification(bool HasDetails) : INotification;
