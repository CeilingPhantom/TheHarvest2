namespace TheHarvest.ECS.Components.Tiles
{
    public class ConstructTile : Tile
    {
        public ConstructTile(int x, int y, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Construct, x, y, 0, isAdvancing, advancingType, cycleTime)
        {}
    }
}