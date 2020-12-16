using System;
using System.Collections.Generic;
using Nez;

namespace TheHarvest.Events
{
    public sealed class EventGroupController
    {
        static readonly Lazy<EventGroupController> lazy = new Lazy<EventGroupController>(() => new EventGroupController());

        public static EventGroupController Instance => lazy.Value;

        Dictionary<string, FastList<EventGroupMember>> groupDict = new Dictionary<string, FastList<EventGroupMember>>();

        private EventGroupController() { }

        public List<string> Groups => new List<string>(this.groupDict.Keys);

        public void RegisterEntityGroup(string name)
        {
            this.groupDict.Add(name, new FastList<EventGroupMember>());
        }

        public void SubscribeToGroup(string name, params EventGroupMember[] members)
        {
            this.groupDict[name].AddRange(members);
        }

        public void PublishToGroups(Event e, params string[] names)
        {
            HashSet<EventGroupMember> members = new HashSet<EventGroupMember>();
            foreach (var name in names)
                if (this.groupDict.ContainsKey(name))
                    for (var i = 0; i < this.groupDict[name].Length; ++i)
                        members.Add(this.groupDict[name][i]);
            // members won't receive duplicate events
            foreach (var member in members)
                member.SendEvent(e);
        }
    }
}
