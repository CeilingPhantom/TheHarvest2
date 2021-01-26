namespace TheHarvest.Events
{
    public interface IEventSubscriber
    {
        void SubscribeTo<T>() where T : IEvent;
        
        void SendEvent(IEvent e);

        #region Event Processing

        void ProcessEvent(AddMoneyEvent e);
        void ProcessEvent(AddTileEvent e);

        #endregion
    }
}