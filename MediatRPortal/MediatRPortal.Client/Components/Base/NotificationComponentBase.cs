using MediatR;
using Microsoft.AspNetCore.Components;

namespace MediatRPortal.Client.Components.Base;

// Has to be generic so MediatR can register the open generic type INotificationHandler<>
public class NotificationComponentBase<TNotification> : ComponentBase, IDisposable, INotificationHandler<TNotification> where TNotification : INotification
{
    // Unique key for each component to store dispose actions
    public readonly string componentId = $"{Guid.NewGuid()}";

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
        var eventHandlersKey = typeof(TNestedNotification);

        if (NotificationComponentBaseHelper.EventHandlers.TryGetValue(eventHandlersKey, out var handlers))
        {
            handlers.Add(handler);
        }
        else
        {
            NotificationComponentBaseHelper.EventHandlers[eventHandlersKey] = [handler];
        }

        //NotificationComponentBaseHelper.Disposes.Add(() =>
        //{
        //    if (NotificationComponentBaseHelper.EventHandlers.TryGetValue(eventHandlersKey, out var handlers))
        //    {
        //        handlers.Remove(handler);
        //        if (handlers.Count == 0)
        //        {
        //            NotificationComponentBaseHelper.EventHandlers.Remove(eventHandlersKey);
        //        }
        //    }
        //});

        if (NotificationComponentBaseHelper.Disposes.TryGetValue(componentId, out var disposes))
        {
            disposes.Add(() =>
            {
                if (NotificationComponentBaseHelper.EventHandlers.TryGetValue(eventHandlersKey, out var handlers))
                {
                    handlers.Remove(handler);
                    if (handlers.Count == 0)
                    {
                        NotificationComponentBaseHelper.EventHandlers.Remove(eventHandlersKey);
                    }
                }
            });
        }
        else
        {
            NotificationComponentBaseHelper.Disposes[componentId] = [() =>
            {
                if (NotificationComponentBaseHelper.EventHandlers.TryGetValue(eventHandlersKey, out var handlers))
                {
                    handlers.Remove(handler);
                    if (handlers.Count == 0)
                    {
                        NotificationComponentBaseHelper.EventHandlers.Remove(eventHandlersKey);
                    }
                }
            }];
        }
    }

    public Task Handle(TNotification notification, CancellationToken cancellationToken)
    {
        var eventHandlersKey = typeof(TNotification);

        if (NotificationComponentBaseHelper.EventHandlers.TryGetValue(eventHandlersKey, out var results))
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
        //foreach (var dispose in NotificationComponentBaseHelper.Disposes)
        //{
        //    dispose();
        //}
        //NotificationComponentBaseHelper.Disposes.Clear();

        if (NotificationComponentBaseHelper.Disposes.TryGetValue(componentId, out var disposes))
        {
            foreach (var dispose in disposes)
            {
                dispose();
            }
            NotificationComponentBaseHelper.Disposes.Remove(componentId);
        }

        GC.SuppressFinalize(this);
    }
}

/// <summary>
/// Helper class to store event handlers and dispose them when the component is disposed
/// </summary>
public static class NotificationComponentBaseHelper
{
    /// <summary>
    /// Dictionary to store event handlers for each notification type in every component
    /// </summary>
    public static readonly Dictionary<Type, List<Delegate>> EventHandlers = [];

    ///// <summary>
    ///// List to store dispose actions for every component
    ///// </summary>
    //public static readonly List<Action> Disposes = [];


    /// <summary>
    /// Dictionary to store dispose actions for belonging component
    /// </summary>
    public static readonly Dictionary<string, List<Action>> Disposes = [];
}