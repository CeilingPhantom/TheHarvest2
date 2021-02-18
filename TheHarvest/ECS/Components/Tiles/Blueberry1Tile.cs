namespace TheHarvest.ECS.Components.Tiles
{
    public class Blueberry1Tile : Tile
    {
        public Blueberry1Tile(int x, int y, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Blueberry1, x, y, 10, isAdvancing, advancingType, cycleTime)
        {}
    }
}