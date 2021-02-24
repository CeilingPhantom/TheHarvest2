namespace TheHarvest.Events
{
    public interface IEventSubscriber
    {
        void SubscribeTo<T>() where T : IEvent;
        
        void SendEvent(IEvent e);

        #region Event Processing

        void ProcessEvent(AddMoneyEvent e);
        void ProcessEvent(TileSelectionEvent e);
        void ProcessEvent(TentativeFarmGridOnEvent e);
        void ProcessEvent(TentativeFarmGridOffEvent e);
        void ProcessEvent(TentativeFarmGridUndo e);
        void ProcessEvent(TentativeFarmGridRedo e);

        #endregion
    }
}