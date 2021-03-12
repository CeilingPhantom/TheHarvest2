using TheHarvest.ECS.Components.Farm;

namespace TheHarvest.ECS.Components.Tiles
{
    public class Carrot2Tile : Tile
    {
        public Carrot2Tile(int x, int y, Grid grid, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Carrot2, x, y, grid, 20, isAdvancing, advancingType, cycleTime)
        {}
    }
}