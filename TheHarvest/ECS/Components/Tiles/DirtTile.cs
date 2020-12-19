namespace TheHarvest.ECS.Components
{
    public class DirtTile : Tile
    {
        public DirtTile(int x, int y, float cycleTime, bool isAdvancing, TileType advancingType) 
        : base(TileType.Dirt, x, y, cycleTime, isAdvancing, advancingType)
        {}
    }
}