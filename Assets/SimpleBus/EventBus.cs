using System.Collections.Generic;
using UnityEngine;

namespace SimpleBus
{
    public static class EventBus<T> where T : IEvent
    {
        static readonly HashSet<IEventBinding<T>> bindings = new();

        public static void Register(IEventBinding<T> binding)
        {
            bindings.Add(binding);
        }

        public static void Deregister(IEventBinding<T> binding)
        {
            bindings.Remove(binding);
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

