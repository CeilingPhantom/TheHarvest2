using TheHarvest.ECS.Components.Farm;

namespace TheHarvest.ECS.Components.Tiles
{
    public class Blueberry3Tile : Tile
    {
        public Blueberry3Tile(int x, int y, Grid grid, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Blueberry3, x, y, grid, 30, isAdvancing, advancingType, cycleTime)
        {}
    }
}