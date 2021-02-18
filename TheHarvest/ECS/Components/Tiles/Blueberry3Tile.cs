namespace TheHarvest.ECS.Components.Tiles
{
    public class Blueberry3Tile : Tile
    {
        public Blueberry3Tile(int x, int y, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Blueberry3, x, y, 30, isAdvancing, advancingType, cycleTime)
        {}
    }
}