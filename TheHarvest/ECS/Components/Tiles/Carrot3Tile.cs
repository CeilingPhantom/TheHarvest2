namespace TheHarvest.ECS.Components.Tiles
{
    public class Carrot3Tile : Tile
    {
        public Carrot3Tile(int x, int y, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Carrot3, x, y, 30, isAdvancing, advancingType, cycleTime)
        {}
    }
}