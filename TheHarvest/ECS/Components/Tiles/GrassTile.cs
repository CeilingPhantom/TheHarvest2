namespace TheHarvest.ECS.Components.Tiles
{
    public class GrassTile : Tile
    {
        public GrassTile(int x, int y, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Grass, x, y, 0, isAdvancing, advancingType, cycleTime)
        {}
    }
}