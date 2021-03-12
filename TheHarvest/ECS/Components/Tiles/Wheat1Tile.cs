using TheHarvest.ECS.Components.Farm;

namespace TheHarvest.ECS.Components.Tiles
{
    public class Wheat1Tile : Tile
    {
        public Wheat1Tile(int x, int y, Grid grid, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Wheat1, x, y, grid, 10, isAdvancing, advancingType, cycleTime)
        {}
    }
}