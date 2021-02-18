namespace TheHarvest.ECS.Components.Tiles
{
    public class Strawberry3Tile : Tile
    {
        public Strawberry3Tile(int x, int y, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Strawberry3, x, y, 30, isAdvancing, advancingType, cycleTime)
        {}
    }
}