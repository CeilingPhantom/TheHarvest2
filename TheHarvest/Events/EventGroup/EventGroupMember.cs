using Nez;

namespace TheHarvest.Events
{
    public abstract class EventGroupMember : Component
    {
        public abstract void SendEvent(Event e);
    }
}