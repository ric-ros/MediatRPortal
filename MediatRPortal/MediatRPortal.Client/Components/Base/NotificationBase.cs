using MediatR;

namespace MediatRPortal.Client.Components.Base;

public record NotificationBase(Guid SessionId) : INotification;