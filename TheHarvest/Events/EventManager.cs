using System;
using System.Collections.Generic;
using Nez;

namespace TheHarvest.Events
{
    public sealed class EventManager : GlobalManager
    {
        static readonly Lazy<EventManager> lazy = new Lazy<EventManager>(() => new EventManager());
        public static EventManager Instance => lazy.Value;

        Dictionary<Type, FastList<IEventSubscriber>> groupDict = new Dictionary<Type, FastList<IEventSubscriber>>();

        FastList<IEvent> nextFrameEvents = new FastList<IEvent>();

        EventManager() : base()
        {
            nextFrameEvents.Add(new AddMoneyEvent(1));
        }

        public void Initialize()
        {
            Core.RegisterGlobalManager(this);
        }

        void RegisterEventGroup<T>() where T : IEvent
        {
            if (!this.groupDict.ContainsKey(typeof(T)))
                this.groupDict.Add(typeof(T), new FastList<IEventSubscriber>());
        }

        public void SubscribeTo<T>(params IEventSubscriber[] subscribers) where T : IEvent
        {
            this.RegisterEventGroup<T>();
            this.groupDict[typeof(T)].AddRange(subscribers);
        }

        public void Publish<T>(T e) where T : IEvent
        {
            for (var i = 0; i < this.groupDict[typeof(T)].Length; ++i)
                this.groupDict[typeof(T)][i].SendEvent(e);
        }

        public override void Update()
        {
            base.Update();
            System.Diagnostics.Debug.WriteLine(nextFrameEvents[0].GetType() == typeof(AddMoneyEvent));
        }
    }
}
