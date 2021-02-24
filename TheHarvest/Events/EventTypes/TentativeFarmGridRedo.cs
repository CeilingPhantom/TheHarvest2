namespace TheHarvest.Events
{
    public class TentativeFarmGridRedo : IEvent
    {
        public TentativeFarmGridRedo()
        {}

        public void Accept(IEventSubscriber subscriber)
        {
            subscriber.ProcessEvent(this);
        }
    }
}