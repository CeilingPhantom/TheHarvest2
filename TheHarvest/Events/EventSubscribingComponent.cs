using System.Collections.Generic;
using Nez;

namespace TheHarvest.Events
{
    public abstract class EventSubscribingComponent : Component, IUpdatable, IEventSubscriber
    {
        Queue<IEvent> events = new Queue<IEvent>();
        // TODO may need this in the future
        // Queue<IEvent> eventsWhileDisabled = new Queue<IEvent>();

        public void SubscribeTo<T>() where T : IEvent
        {
            EventManager.Instance.SubscribeTo<T>(this);
        }
        
        public void SendEvent(IEvent e)
        {
            this.events.Enqueue(e);
        }

        public virtual void Update()
        {
            while (events.Count > 0)
            {
                events.Dequeue().Accept(this);
            }
        }

        #region Event Processing

        public virtual void ProcessEvent(AddMoneyEvent e)
        {}

        public virtual void ProcessEvent(TileSelectionEvent e)
        {}

        public virtual void ProcessEvent(TentativeFarmGridOnEvent e)
        {}

        public virtual void ProcessEvent(TentativeFarmGridOffEvent e)
        {}

        public virtual void ProcessEvent(TentativeFarmGridUndoEvent e)
        {}

        public virtual void ProcessEvent(TentativeFarmGridRedoEvent e)
        {}

        public virtual void ProcessEvent(NewDayEvent e)
        {}

        public virtual void ProcessEvent(NewSeasonEvent e)
        {}

        public virtual void ProcessEvent(NewYearEvent e)
        {}

        #endregion
    }
}