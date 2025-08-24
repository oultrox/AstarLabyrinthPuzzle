using System.Collections.Generic;
using UnityEngine;

namespace SimpleBus
{
    public static class EventBus<T> where T : IEvent
    {
        static readonly HashSet<IEventBinder<T>> bindings = new();

        public static void Register(IEventBinder<T> binder)
        {
            bindings.Add(binder);
        }

        public static void Deregister(IEventBinder<T> binder)
        {
            bindings.Remove(binder);
        }

        public static void Raise(T @event)
        {
            foreach (var eventBinding in bindings)
            {
                eventBinding.OnEvent.Invoke(@event);
                eventBinding.OnEventNoArgs.Invoke();
            }
        }

        static void Clear()
        {
            Debug.Log("Event Binding cleared: " + typeof(T).Name);
            bindings.Clear();
        }
    }
}

