using TheHarvest.ECS.Components.Farm;

namespace TheHarvest.ECS.Components.Tiles
{
    public class Wheat2Tile : Tile
    {
        public Wheat2Tile(int x, int y, Grid grid, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Wheat2, x, y, grid, 20, isAdvancing, advancingType, cycleTime)
        {}
    }
}