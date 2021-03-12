using TheHarvest.ECS.Components.Farm;
using TheHarvest.ECS.Components.Player;

namespace TheHarvest.ECS.Components.Tiles
{
    public class Blueberry1Tile : Crop
    {
        public Blueberry1Tile(int x, int y, Grid grid, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Blueberry1, x, y, grid, 10, PlayerState.Seasons.Summer, isAdvancing, advancingType, cycleTime)
        {}

        protected override int GetYield()
        {
            return 1;
        }
    }
}