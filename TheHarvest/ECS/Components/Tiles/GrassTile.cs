namespace TheHarvest.ECS.Components
{
    public class GrassTile : Tile
    {
        public GrassTile(int x, int y, float cycleTime, bool isAdvancing, TileType advancingType) 
        : base(TileType.Grass, x, y, cycleTime, isAdvancing, advancingType)
        {}
    }
}