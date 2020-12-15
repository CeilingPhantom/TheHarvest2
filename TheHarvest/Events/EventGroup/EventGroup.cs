using Nez;

namespace TheHarvest.Events
{
    public abstract class EventGroup  // game, UI, 
    {
        private FastList<EventGroupMember> _members = new FastList<EventGroupMember>();

        public void SubscribeToGroup(params EventGroupMember[] members)
        {
            _members.AddRange(members);
        }

        public abstract void PublishToSubscribers(Event e);  // e.g. filter who should receive event
    }
}