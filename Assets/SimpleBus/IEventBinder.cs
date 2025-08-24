using System;

namespace SimpleBus
{
    public interface IEventBinder<T>
    {
        public Action<T> OnEvent { get; set; }
        public Action OnEventNoArgs { get; set; }
    }
}