using System.Collections.Generic;
using Nez;

namespace TheHarvest.Events
{
    public abstract class EventGroupMember : Component, IUpdatable
    {
        Queue<Event> queue;
        
        public void SendEvent(Event e)
        {
            this.queue.Enqueue(e);
        }

        public void Update()
        {
            // for current number of events in queue
            // new events may be queued during the update
            for (var c = queue.Count; c > 0; --c)
                ProcessEvent(queue.Dequeue());
        }

        // members decide what events to process and how to process them
        protected abstract void ProcessEvent(Event e);
    }
}