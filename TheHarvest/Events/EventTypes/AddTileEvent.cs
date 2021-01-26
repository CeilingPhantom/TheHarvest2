using TheHarvest.ECS.Components;

namespace TheHarvest.Events
{
    public class AddTileEvent : IEvent
    {
        public Tile Tile;

        public AddTileEvent(Tile tile)
        {
            this.Tile = tile;
        }

        public void Accept(IEventSubscriber subscriber)
        {
            subscriber.ProcessEvent(this);
        }
    }
}