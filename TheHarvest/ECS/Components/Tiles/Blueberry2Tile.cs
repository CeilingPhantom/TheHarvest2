using TheHarvest.ECS.Components.Farm;
using TheHarvest.ECS.Components.Player;

namespace TheHarvest.ECS.Components.Tiles
{
    public class Blueberry2Tile : Crop
    {
        public Blueberry2Tile(int x, int y, Grid grid, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Blueberry2, x, y, grid, 20, PlayerState.Seasons.Spring, isAdvancing, advancingType, cycleTime)
        {}

        protected override int GetYield()
        {
            return 2;
        }
    }
}