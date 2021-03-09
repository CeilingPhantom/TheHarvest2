namespace TheHarvest.Events
{
    public class NewDayEvent : IEvent
    {
        public NewDayEvent()
        {}

        public void Accept(IEventSubscriber subscriber)
        {
            subscriber.ProcessEvent(this);
        }
    }
}