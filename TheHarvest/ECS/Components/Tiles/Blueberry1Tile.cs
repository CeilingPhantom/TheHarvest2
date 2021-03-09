using TheHarvest.ECS.Components.Player;
using TheHarvest.Events;

namespace TheHarvest.ECS.Components.Tiles
{
    public class Blueberry1Tile : Crop
    {
        public Blueberry1Tile(int x, int y, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Blueberry1, x, y, 10, PlayerState.Seasons.Summer, isAdvancing, advancingType, cycleTime)
        {}

        protected override int GetYield()
        {
            return 1;
        }
    }
}