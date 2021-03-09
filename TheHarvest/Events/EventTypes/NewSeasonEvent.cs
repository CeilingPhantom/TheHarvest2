namespace TheHarvest.Events
{
    public class NewSeasonEvent : IEvent
    {
        public NewSeasonEvent()
        {}

        public void Accept(IEventSubscriber subscriber)
        {
            subscriber.ProcessEvent(this);
        }
    }
}