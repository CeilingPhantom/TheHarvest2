namespace TheHarvest.ECS.Components.Tiles
{
    public class Strawberry1Tile : Tile
    {
        public Strawberry1Tile(int x, int y, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Strawberry1, x, y, 10, isAdvancing, advancingType, cycleTime)
        {}
    }
}