using TheHarvest.ECS.Components.Farm;

namespace TheHarvest.ECS.Components.Tiles
{
    public class ConstructTile : Tile
    {
        public ConstructTile(int x, int y, Grid grid, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Construct, x, y, grid, 0, isAdvancing, advancingType, cycleTime)
        {}
    }
}