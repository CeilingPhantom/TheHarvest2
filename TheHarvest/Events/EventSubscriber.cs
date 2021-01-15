using System.Collections.Generic;
using Nez;

namespace TheHarvest.Events
{
    public abstract class EventSubscriber : Component, IUpdatable
    {
        Queue<IEvent> queue = new Queue<IEvent>();

        public void SubscribeTo<T>() where T : IEvent
        {
            EventManager.Instance.AddSubscribers<T>(this);
        }
        
        public void SendEvent(IEvent e)
        {
            this.queue.Enqueue(e);
        }

        public virtual void Update()
        {
            // for current number of events in queue
            // new events may be queued during the update
            for (var c = queue.Count; c > 0; --c)
                queue.Dequeue().Accept(this);
        }

        #region Event Processing

        protected internal virtual void ProcessEvent(AddMoneyEvent e)
        {}

        #endregion
    }
}