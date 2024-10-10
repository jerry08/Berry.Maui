using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;

namespace Berry.Maui.Extensions;

// https://github.com/dotnet/maui/blob/main/src/Controls/src/Core/DispatcherExtensions.cs
public static class DispatcherExtensions
{
    public static void DispatchIfRequired(this IDispatcher? dispatcher, Action action)
    {
        dispatcher = EnsureDispatcher(dispatcher);
        if (dispatcher.IsDispatchRequired)
        {
            dispatcher.Dispatch(action);
        }
        else
        {
            action();
        }
    }

    public static Task DispatchIfRequiredAsync(this IDispatcher? dispatcher, Action action)
    {
        dispatcher = EnsureDispatcher(dispatcher);
        if (dispatcher.IsDispatchRequired)
        {
            return dispatcher.DispatchAsync(action);
        }
        else
        {
            action();
            return Task.CompletedTask;
        }
    }

    public static Task DispatchIfRequiredAsync(this IDispatcher? dispatcher, Func<Task> action)
    {
        dispatcher = EnsureDispatcher(dispatcher);
        if (dispatcher.IsDispatchRequired)
        {
            return dispatcher.DispatchAsync(action);
        }
        else
        {
            return action();
        }
    }

    static IDispatcher EnsureDispatcher(IDispatcher? dispatcher)
    {
        if (dispatcher is not null)
            return dispatcher;

        // maybe this thread has a dispatcher
        if (Dispatcher.GetForCurrentThread() is IDispatcher globalDispatcher)
            return globalDispatcher;

        // try looking on the app
        if (Application.Current?.Dispatcher is IDispatcher appDispatcher)
            return appDispatcher;

        // no dispatchers found at all
        throw new InvalidOperationException(
            "The dispatcher was not found and the current application does not have a dispatcher."
        );
    }
}
