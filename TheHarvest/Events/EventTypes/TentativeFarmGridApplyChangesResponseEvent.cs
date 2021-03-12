namespace TheHarvest.Events
{
    public class TentativeFarmGridApplyChangesResponseEvent : IEvent
    {
        public bool CanApply { get; private set; }

        public TentativeFarmGridApplyChangesResponseEvent(bool canApply)
        {
            this.CanApply = canApply;
        }

        public void Accept(IEventSubscriber subscriber)
        {
            subscriber.ProcessEvent(this);
        }
    }
}