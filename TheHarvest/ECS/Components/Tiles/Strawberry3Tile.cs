using TheHarvest.ECS.Components.Farm;

namespace TheHarvest.ECS.Components.Tiles
{
    public class Strawberry3Tile : Tile
    {
        public Strawberry3Tile(int x, int y, Grid grid, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Strawberry3, x, y, grid, 30, isAdvancing, advancingType, cycleTime)
        {}
    }
}