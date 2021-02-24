namespace TheHarvest.Events
{
    public class TentativeFarmGridUndo : IEvent
    {
        public TentativeFarmGridUndo()
        {}

        public void Accept(IEventSubscriber subscriber)
        {
            subscriber.ProcessEvent(this);
        }
    }
}