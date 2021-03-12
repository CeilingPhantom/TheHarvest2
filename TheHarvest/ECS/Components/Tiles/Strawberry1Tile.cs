using TheHarvest.ECS.Components.Farm;

namespace TheHarvest.ECS.Components.Tiles
{
    public class Strawberry1Tile : Tile
    {
        public Strawberry1Tile(int x, int y, Grid grid, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Strawberry1, x, y, grid, 10, isAdvancing, advancingType, cycleTime)
        {}
    }
}