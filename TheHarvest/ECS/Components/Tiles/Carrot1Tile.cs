namespace TheHarvest.ECS.Components.Tiles
{
    public class Carrot1Tile : Tile
    {
        public Carrot1Tile(int x, int y, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Carrot1, x, y, 10, isAdvancing, advancingType, cycleTime)
        {}
    }
}