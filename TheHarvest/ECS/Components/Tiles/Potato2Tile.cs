using TheHarvest.ECS.Components.Farm;

namespace TheHarvest.ECS.Components.Tiles
{
    public class Potato2Tile : Tile
    {
        public Potato2Tile(int x, int y, Grid grid, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Potato2, x, y, grid, 20, isAdvancing, advancingType, cycleTime)
        {}
    }
}