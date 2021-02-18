namespace TheHarvest.ECS.Components.Tiles
{
    public class Potato1Tile : Tile
    {
        public Potato1Tile(int x, int y, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Potato1, x, y, 10, isAdvancing, advancingType, cycleTime)
        {}
    }
}