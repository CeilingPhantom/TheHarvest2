namespace TheHarvest.ECS.Components.Tiles
{
    public class Potato3Tile : Tile
    {
        public Potato3Tile(int x, int y, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Potato3, x, y, 30, isAdvancing, advancingType, cycleTime)
        {}
    }
}