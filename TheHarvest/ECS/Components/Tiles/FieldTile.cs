using TheHarvest.FileManagers;

namespace TheHarvest.ECS.Components.Tiles
{
    public class FieldTile : Tile
    {
        public FieldTile(int x, int y, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Field, x, y, 0, isAdvancing, advancingType, cycleTime)
        {}

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            this.SetSprite(TilesetSpriteManager.Instance.GetSprite(this.TileType));
        }
    }
}