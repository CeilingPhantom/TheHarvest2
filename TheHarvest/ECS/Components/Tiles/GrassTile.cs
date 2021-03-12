using TheHarvest.ECS.Components.Farm;

namespace TheHarvest.ECS.Components.Tiles
{
    public class GrassTile : Tile
    {
        public GrassTile(int x, int y, Grid grid, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Grass, x, y, grid, 0, isAdvancing, advancingType, cycleTime)
        {}
    }
}