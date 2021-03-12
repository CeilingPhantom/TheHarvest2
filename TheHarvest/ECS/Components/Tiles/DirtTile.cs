using TheHarvest.ECS.Components.Farm;

namespace TheHarvest.ECS.Components.Tiles
{
    public class DirtTile : Tile
    {
        public DirtTile(int x, int y, Grid grid, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Dirt, x, y, grid, 0, isAdvancing, advancingType, cycleTime)
        {}
    }
}