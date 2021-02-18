namespace TheHarvest.ECS.Components.Tiles
{
    public class Potato2Tile : Tile
    {
        public Potato2Tile(int x, int y, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Potato2, x, y, 20, isAdvancing, advancingType, cycleTime)
        {}
    }
}