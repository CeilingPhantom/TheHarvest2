namespace TheHarvest.Events
{
    public class NewYearEvent : IEvent
    {
        public NewYearEvent()
        {}

        public void Accept(IEventSubscriber subscriber)
        {
            subscriber.ProcessEvent(this);
        }
    }
}