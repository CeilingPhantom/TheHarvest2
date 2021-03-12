namespace TheHarvest.Events
{
    public class TentativeFarmGridOffEvent : IEvent
    {
        public bool ApplyChanges { get; private set; }

        public TentativeFarmGridOffEvent(bool applyChanges)
        {
            this.ApplyChanges = applyChanges;
        }

        public void Accept(IEventSubscriber subscriber)
        {
            subscriber.ProcessEvent(this);
        }
    }
}