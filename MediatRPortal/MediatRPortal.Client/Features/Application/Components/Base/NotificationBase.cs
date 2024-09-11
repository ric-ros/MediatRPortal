using MediatR;

namespace MediatRPortal.Client.Features.Application.Components.Base;

public record NotificationBase(Guid SessionId) : INotification;