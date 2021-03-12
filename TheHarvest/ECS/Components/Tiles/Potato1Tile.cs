using TheHarvest.ECS.Components.Farm;

namespace TheHarvest.ECS.Components.Tiles
{
    public class Potato1Tile : Tile
    {
        public Potato1Tile(int x, int y, Grid grid, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Potato1, x, y, grid, 10, isAdvancing, advancingType, cycleTime)
        {}
    }
}