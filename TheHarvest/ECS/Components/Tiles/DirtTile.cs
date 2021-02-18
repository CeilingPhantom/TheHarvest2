namespace TheHarvest.ECS.Components.Tiles
{
    public class DirtTile : Tile
    {
        public DirtTile(int x, int y, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Dirt, x, y, 0, isAdvancing, advancingType, cycleTime)
        {}
    }
}