using System.Collections.Generic;
using Nez;

namespace TheHarvest.Events
{
    public abstract class EventSubscribingComponent : Component, IUpdatable, IEventSubscriber
    {
        Queue<IEvent> queue = new Queue<IEvent>();

        public void SubscribeTo<T>() where T : IEvent
        {
            EventManager.Instance.SubscribeTo<T>(this);
        }
        
        public void SendEvent(IEvent e)
        {
            this.queue.Enqueue(e);
        }

        public virtual void Update()
        {
            while (queue.Count > 0)
                queue.Dequeue().Accept(this);
        }

        #region Event Processing

        public virtual void ProcessEvent(AddMoneyEvent e)
        {}

        public virtual void ProcessEvent(TileSelectionEvent e)
        {}

        public virtual void ProcessEvent(EditFarmOnEvent e)
        {}

        public virtual void ProcessEvent(EditFarmOffEvent e)
        {}

        #endregion
    }
}