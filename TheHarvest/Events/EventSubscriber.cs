using System.Collections.Generic;
using Nez;

namespace TheHarvest.Events
{
    public abstract class EventSubscriber : Component, IUpdatable
    {
        Queue<IEvent> queue;
        
        public void SendEvent(IEvent e)
        {
            this.queue.Enqueue(e);
        }

        public void Update()
        {
            // for current number of events in queue
            // new events may be queued during the update
            for (var c = queue.Count; c > 0; --c)
                queue.Dequeue().Accept(this);
        }

        // subscribers decide what events to process and how to process them
        internal abstract void ProcessEvent(AddMoneyEvent e);
    }
}