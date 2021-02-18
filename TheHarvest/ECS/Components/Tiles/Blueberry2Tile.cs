namespace TheHarvest.ECS.Components.Tiles
{
    public class Blueberry2Tile : Tile
    {
        public Blueberry2Tile(int x, int y, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Blueberry2, x, y, 20, isAdvancing, advancingType, cycleTime)
        {}
    }
}