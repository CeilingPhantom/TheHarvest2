namespace TheHarvest.Events
{
    public class TentativeFarmGridOffEvent : IEvent
    {
        public bool ApplyChanges { get; private set; }

        public TentativeFarmGridOffEvent(bool ApplyChanges)
        {
            this.ApplyChanges = ApplyChanges;
        }

        public void Accept(IEventSubscriber subscriber)
        {
            subscriber.ProcessEvent(this);
        }
    }
}