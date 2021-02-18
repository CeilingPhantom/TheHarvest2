namespace TheHarvest.ECS.Components.Tiles
{
    public class Wheat3Tile : Tile
    {
        public Wheat3Tile(int x, int y, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Wheat3, x, y, 30, isAdvancing, advancingType, cycleTime)
        {}
    }
}