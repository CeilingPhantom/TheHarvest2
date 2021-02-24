namespace TheHarvest.Events
{
    public class TentativeFarmGridOnEvent : IEvent
    {
        public TentativeFarmGridOnEvent()
        {}

        public void Accept(IEventSubscriber subscriber)
        {
            subscriber.ProcessEvent(this);
        }
    }
}