
namespace TheHarvest.Events
{
    public class EditFarmOnEvent : IEvent
    {
        public EditFarmOnEvent()
        {}

        public void Accept(IEventSubscriber subscriber)
        {
            subscriber.ProcessEvent(this);
        }
    }
}