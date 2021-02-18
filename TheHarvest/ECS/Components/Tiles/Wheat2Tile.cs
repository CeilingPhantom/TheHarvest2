namespace TheHarvest.ECS.Components.Tiles
{
    public class Wheat2Tile : Tile
    {
        public Wheat2Tile(int x, int y, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Wheat2, x, y, 20, isAdvancing, advancingType, cycleTime)
        {}
    }
}