using TheHarvest.ECS.Components.Farm;

namespace TheHarvest.ECS.Components.Tiles
{
    public class Carrot3Tile : Tile
    {
        public Carrot3Tile(int x, int y, Grid grid, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Carrot3, x, y, grid, 30, isAdvancing, advancingType, cycleTime)
        {}
    }
}