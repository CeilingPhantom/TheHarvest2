namespace TheHarvest.Events
{
    public interface IEventSubscriber
    {
        void SubscribeTo<T>() where T : IEvent;
        void Publish<T>(T e) where T : IEvent;
        void SendEvent(IEvent e);
        void ProcessEvent(AddMoneyEvent e);
        void ProcessEvent(TileSelectionEvent e);
        void ProcessEvent(TentativeFarmGridApplyChangesRequestEvent e);
        void ProcessEvent(TentativeFarmGridApplyChangesResponseEvent e);
        void ProcessEvent(TentativeFarmGridOnEvent e);
        void ProcessEvent(TentativeFarmGridOffEvent e);
        void ProcessEvent(TentativeFarmGridUndoEvent e);
        void ProcessEvent(TentativeFarmGridRedoEvent e);
        void ProcessEvent(NewDayEvent e);
        void ProcessEvent(NewSeasonEvent e);
        void ProcessEvent(NewYearEvent e);
    }
}