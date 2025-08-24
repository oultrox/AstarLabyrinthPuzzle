using System;

namespace SimpleBus
{
    public class EventBinder<T> : IEventBinder<T> where T : IEvent
    {
        Action<T> onEvent = _ => { };
        Action onEventNoArgs = () => { };
    
        Action<T> IEventBinder<T>.OnEvent { get => onEvent; set => onEvent = value; }
        Action IEventBinder<T>.OnEventNoArgs { get => onEventNoArgs; set => onEventNoArgs = value;}


        public EventBinder(Action<T> onEvent)
        {
            this.onEvent = onEvent;
        }

        public EventBinder(Action onEventNoArgs)
        {
            this.onEventNoArgs = onEventNoArgs;
        }
    
        public void Add(Action<T> onEvent)
        {
            this.onEvent += onEvent;
        }

        public void Remove(Action<T> onEvent)
        {
            this.onEvent -= onEvent;
        }

        public void Add(Action onEvent)
        {
            onEventNoArgs += onEvent;
        }

        public void Remove(Action onEvent)
        {
            onEventNoArgs -= onEvent;
        }
    }
}