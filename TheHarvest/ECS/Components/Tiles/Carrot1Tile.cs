using TheHarvest.ECS.Components.Farm;

namespace TheHarvest.ECS.Components.Tiles
{
    public class Carrot1Tile : Tile
    {
        public Carrot1Tile(int x, int y, Grid grid, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Carrot1, x, y, grid, 10, isAdvancing, advancingType, cycleTime)
        {}
    }
}