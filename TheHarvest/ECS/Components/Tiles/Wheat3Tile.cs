using TheHarvest.ECS.Components.Farm;

namespace TheHarvest.ECS.Components.Tiles
{
    public class Wheat3Tile : Tile
    {
        public Wheat3Tile(int x, int y, Grid grid, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Wheat3, x, y, grid, 30, isAdvancing, advancingType, cycleTime)
        {}
    }
}