using MediatR;
using Microsoft.AspNetCore.Components;

namespace MediatRPortal.Client.Features.Application.Components.Base;

/// <summary>
/// Base component for handling notifications in a MediatR-based system.
/// </summary>
public class NotificationComponentBase<TNotification> : ComponentBase, IDisposable, INotificationHandler<TNotification>
    where TNotification : NotificationBase
{
    [CascadingParameter]
    protected Guid SessionId { get; set; }

    /// <summary>
    /// Unique identifier for the component.
    /// </summary>
    protected readonly Guid ComponentId = Guid.NewGuid();

    protected override void OnInitialized()
    {
        base.OnInitialized();
        RegisterNotificationHandlers();
    }

    /// <summary>
    /// Override this method in derived classes to register specific notification handlers.
    /// </summary>
    protected virtual void RegisterNotificationHandlers() { }

    /// <summary>
    /// Registers a notification handler for a specific notification type.
    /// </summary>
    protected void RegisterNotificationHandler<TNestedNotification>(params Action<TNestedNotification>[] handlers)
        where TNestedNotification : NotificationBase
    {
        var eventHandlersKey = new KeyType(SessionId, typeof(TNestedNotification));

        foreach (var handler in handlers)
        {
            NotificationComponentBaseHelper.AddEventHandler(eventHandlersKey, handler);
            NotificationComponentBaseHelper.AddDisposeAction(ComponentId, () =>
            {
                NotificationComponentBaseHelper.RemoveEventHandler(eventHandlersKey, handler);
            });
        }
    }

    /// <summary>
    /// Handled by MediatR.
    /// </summary>
    public Task Handle(TNotification notification, CancellationToken cancellationToken)
    {
        var eventHandlersKey = new KeyType(notification.SessionId, notification.GetType());
        NotificationComponentBaseHelper.InvokeHandlers(eventHandlersKey, notification);
        return Task.CompletedTask;
    }



    public void Dispose()
    {
        NotificationComponentBaseHelper.ExecuteDisposeActions(ComponentId);
        GC.SuppressFinalize(this);
    }
}

/// <summary>
/// Represents a unique key for storing notification handlers.
/// </summary>
public record KeyType(Guid SessionId, Type NotificationType);

/// <summary>
/// Helper class to store event handlers and dispose actions for components
/// </summary>
public static class NotificationComponentBaseHelper
{
    /// <summary>
    /// Dictionary to store event handlers for each component. IT WILL BECOME PRIVATE OUTSIDE THE POC.
    /// </summary>
    public static readonly Dictionary<KeyType, List<Delegate>> EventHandlers = [];

    /// <summary>
    /// Dictionary to store dispose actions for each component. IT WILL BECOME PRIVATE OUTSIDE THE POC.
    /// </summary>
    public static readonly Dictionary<Guid, List<Action>> DisposeActions = [];
    private static readonly object _lockObject = new();

    public static void AddEventHandler(KeyType key, Delegate handler)
    {
        lock (_lockObject)
        {
            if (!EventHandlers.TryGetValue(key, out var handlers))
            {
                handlers = [];
                EventHandlers[key] = handlers;
            }
            handlers.Add(handler);
        }
    }

    public static void RemoveEventHandler(KeyType key, Delegate handler)
    {
        lock (_lockObject)
        {
            if (EventHandlers.TryGetValue(key, out var handlers))
            {
                handlers.Remove(handler);
                if (handlers.Count is 0)
                {
                    EventHandlers.Remove(key);
                }
            }
        }
    }

    public static void AddDisposeAction(Guid componentId, Action disposeAction)
    {
        lock (_lockObject)
        {
            if (!DisposeActions.TryGetValue(componentId, out var actions))
            {
                actions = [];
                DisposeActions[componentId] = actions;
            }

            actions.Add(disposeAction);
        }
    }

    public static void ExecuteDisposeActions(Guid componentId)
    {
        lock (_lockObject)
        {
            if (DisposeActions.TryGetValue(componentId, out var actions))
            {
                foreach (var action in actions)
                {
                    action();
                }

                DisposeActions.Remove(componentId);
            }
        }
    }

    public static void InvokeHandlers(KeyType key, object notification)
    {
        List<Delegate> handlersToInvoke;

        lock (_lockObject)
        {
            if (!EventHandlers.TryGetValue(key, out var handlers))
            {
                return;
            }
            handlersToInvoke = [.. handlers];
        }

        foreach (var handler in handlersToInvoke)
        {
            handler.DynamicInvoke(notification);
        }
    }
}