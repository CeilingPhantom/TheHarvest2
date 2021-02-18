namespace TheHarvest.ECS.Components.Tiles
{
    public class Carrot2Tile : Tile
    {
        public Carrot2Tile(int x, int y, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Carrot2, x, y, 20, isAdvancing, advancingType, cycleTime)
        {}
    }
}