using System.Collections.Generic;

namespace TheHarvest.Events
{
    public class EventGroupController
    {
        private static EventGroupController _instance = new EventGroupController();
        public static EventGroupController Controller => _instance;

        private Dictionary<string, EventGroup> _groupDict = new Dictionary<string, EventGroup>();

        private EventGroupController() { }

        public List<string> Groups => new List<string>(_groupDict.Keys);
        
        public void RegisterEntityGroup(string name, EventGroup group)
        {
            _groupDict.Add(name, group);
        }

        public void SubscribeToGroup(string name, params EventGroupMember[] members)
        {
            _groupDict[name].SubscribeToGroup(members);
        }

        public void PublishToGroups(Event e, params string[] names)
        {
            // if names.Length = 0, publish to all
        }
    }
}
