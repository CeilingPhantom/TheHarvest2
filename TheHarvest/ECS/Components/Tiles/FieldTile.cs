using TheHarvest.ECS.Components.Farm;

namespace TheHarvest.ECS.Components.Tiles
{
    public class FieldTile : Tile
    {
        public FieldTile(int x, int y, Grid grid, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Field, x, y, grid, 0, isAdvancing, advancingType, cycleTime)
        {}
    }
}