namespace TheHarvest.Events
{
    public class EditFarmOffEvent : IEvent
    {
        public bool ApplyChanges { get; private set; }

        public EditFarmOffEvent(bool ApplyChanges)
        {
            this.ApplyChanges = ApplyChanges;
        }

        public void Accept(IEventSubscriber subscriber)
        {
            subscriber.ProcessEvent(this);
        }
    }
}