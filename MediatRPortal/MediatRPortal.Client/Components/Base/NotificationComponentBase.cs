using MediatR;
using MediatRPortal.Client.Features.Designer.Notifications;
using MediatRPortal.Client.Features.Routes.Notifications;
using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace MediatRPortal.Client.Components.Base;

public class NotificationComponentBase<TNotification> : ComponentBase, IDisposable, INotificationHandler<TNotification> where TNotification : INotification
{
    protected override void OnInitialized()
    {
        base.OnInitialized();
        RegisterNotificationHandlers();
    }

    /// <summary>
    /// Override this method in derived classes to register specific notification handlers
    /// </summary>
    protected virtual void RegisterNotificationHandlers() { }

    protected void RegisterNotificationHandler<TNestedNotification>(Action<TNestedNotification> handler)
        where TNestedNotification : INotification
    {
        //NotificationComponentBaseHelper.EventHandlers[typeof(TNestedNotification)].Add(handler);
        if (NotificationComponentBaseHelper.EventHandlers.TryGetValue(typeof(TNestedNotification), out var handlers))
        {
            handlers.Add(handler);
        }
        else
        {
            NotificationComponentBaseHelper.EventHandlers[typeof(TNestedNotification)] = [handler];
        }

        NotificationComponentBaseHelper.Disposes.Add(() =>
        {
            //NotificationComponentBaseHelper.EventHandlers.Remove(typeof(TNestedNotification));
            //NotificationComponentBaseHelper.EventHandlers[typeof(TNestedNotification)].Remove(handler);

            if (NotificationComponentBaseHelper.EventHandlers.TryGetValue(typeof(TNestedNotification), out var handlers))
            {
                handlers.Remove(handler);
                if (handlers.Count == 0)
                {
                    NotificationComponentBaseHelper.EventHandlers.Remove(typeof(TNestedNotification));
                }
            }
        });
    }

    public Task Handle(TNotification notification, CancellationToken cancellationToken)
    {
        var notificationType = notification.GetType();
        if (NotificationComponentBaseHelper.EventHandlers.TryGetValue(notificationType, out var results))
        {
            foreach (var result in results)
            {
                var actionResult = result as Action<TNotification>;
                actionResult?.Invoke(notification);
            }
        }

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        foreach (var dispose in NotificationComponentBaseHelper.Disposes)
        {
            dispose();
        }
        NotificationComponentBaseHelper.Disposes.Clear();

        GC.SuppressFinalize(this);
    }
}

/// <summary>
/// Helper class to store event handlers and dispose them when the component is disposed
/// </summary>
public static class NotificationComponentBaseHelper
{
    public static readonly Dictionary<Type, List<Delegate>> EventHandlers = [];
    public static readonly List<Action> Disposes = [];
}