using TheHarvest.ECS.Components.Tiles;

namespace TheHarvest.Events
{
    public class TileSelectionEvent : IEvent
    {
        public TileType TileType { get; private set; }

        public TileSelectionEvent(TileType tileType)
        {
            this.TileType = tileType;
        }

        public void Accept(IEventSubscriber subscriber)
        {
            subscriber.ProcessEvent(this);
        }
    }
}