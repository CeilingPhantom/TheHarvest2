using System.Collections.Generic;
using Nez;

using TheHarvest.Events;
using TheHarvest.Scenes;
using TheHarvest.Util.Input;

namespace TheHarvest.ECS.Components.UI
{
    public abstract class BaseUI : UICanvas, IInputable, IEventSubscriber
    {
        Queue<IEvent> events = new Queue<IEvent>();

        public BaseUI() : base()
        {
            this.RenderLayer = FarmScene.UIRenderLayer;
            // remember to register subclasses to input manager
        }

        public override void Update()
        {
            base.Update();
            while (events.Count > 0)
            {
                events.Dequeue().Accept(this);
            }
        }

        #region IInputable

        public abstract bool InputCollision();

        #endregion

        #region Event Processing

        public void SubscribeTo<T>() where T : IEvent
        {
            EventManager.Instance.SubscribeTo<T>(this);
        }

        public void Publish<T>(T e) where T : IEvent
        {
            EventManager.Instance.Publish(e);
        }
        
        public void SendEvent(IEvent e)
        {
            this.events.Enqueue(e);
        }

        public virtual void ProcessEvent(AddMoneyEvent e)
        {}

        public virtual void ProcessEvent(TileSelectionEvent e)
        {}

        public virtual void ProcessEvent(TentativeFarmGridApplyChangesRequestEvent e)
        {}

        public virtual void ProcessEvent(TentativeFarmGridApplyChangesResponseEvent e)
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