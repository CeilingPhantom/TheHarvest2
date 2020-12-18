namespace TheHarvest.ECS.Components
{
    public class DirtTile : Tile
    {
        public DirtTile(Farm farm, int x, int y, float cycleTime, bool isAdvancing, TileType advancingType) 
        : base(farm, TileType.Dirt, x, y, cycleTime, isAdvancing, advancingType)
        {}
    }
}