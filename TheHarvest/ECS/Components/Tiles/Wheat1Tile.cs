namespace TheHarvest.ECS.Components.Tiles
{
    public class Wheat1Tile : Tile
    {
        public Wheat1Tile(int x, int y, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Wheat1, x, y, 10, isAdvancing, advancingType, cycleTime)
        {}
    }
}