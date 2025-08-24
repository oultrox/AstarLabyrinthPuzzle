using SimpleBus;

namespace MazeFinder.Scripts.Events
{
    public struct TestEvent : IEvent
    {
    
    }

    public struct PlayerEvent : IEvent
    {
        public bool isActive;
    }
}