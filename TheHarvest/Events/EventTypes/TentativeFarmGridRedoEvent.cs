namespace TheHarvest.Events
{
    public class TentativeFarmGridRedoEvent : IEvent
    {
        public TentativeFarmGridRedoEvent()
        {}

        public void Accept(IEventSubscriber subscriber)
        {
            subscriber.ProcessEvent(this);
        }
    }
}