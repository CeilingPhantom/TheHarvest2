namespace TheHarvest.Events
{
    public class TentativeFarmGridApplyChangesRequestEvent : IEvent
    {
        public TentativeFarmGridApplyChangesRequestEvent()
        {}

        public void Accept(IEventSubscriber subscriber)
        {
            subscriber.ProcessEvent(this);
        }
    }
}