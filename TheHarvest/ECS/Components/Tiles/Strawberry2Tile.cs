namespace TheHarvest.ECS.Components.Tiles
{
    public class Strawberry2Tile : Tile
    {
        public Strawberry2Tile(int x, int y, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Strawberry2, x, y, 20, isAdvancing, advancingType, cycleTime)
        {}
    }
}