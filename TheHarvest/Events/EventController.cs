using System;
using System.Collections.Generic;
using Nez;

namespace TheHarvest.Events
{
    public sealed class EventController
    {
        // possible groups - game, ui
        Dictionary<Type, FastList<EventSubscriber>> groupDict = new Dictionary<Type, FastList<EventSubscriber>>();

        private void RegisterEventGroup<T>() where T : IEvent
        {
            if (!this.groupDict.ContainsKey(typeof(T)))
                this.groupDict.Add(typeof(T), new FastList<EventSubscriber>());
        }

        public void Subscribe<T>(params EventSubscriber[] subscribers) where T : IEvent
        {
            this.RegisterEventGroup<T>();
            this.groupDict[typeof(T)].AddRange(subscribers);
        }

        public void Publish<T>(T e) where T : IEvent
        {
            for (var i = 0; i < this.groupDict[typeof(T)].Length; ++i)
                this.groupDict[typeof(T)][i].SendEvent(e);
        }
    }
}
