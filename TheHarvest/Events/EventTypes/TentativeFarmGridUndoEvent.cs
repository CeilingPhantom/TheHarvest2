namespace TheHarvest.Events
{
    public class TentativeFarmGridUndoEvent : IEvent
    {
        public TentativeFarmGridUndoEvent()
        {}

        public void Accept(IEventSubscriber subscriber)
        {
            subscriber.ProcessEvent(this);
        }
    }
}