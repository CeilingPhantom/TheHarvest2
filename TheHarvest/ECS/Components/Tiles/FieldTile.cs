using TheHarvest.FileManagers;

namespace TheHarvest.ECS.Components.Tiles
{
    public class FieldTile : Tile
    {
        public FieldTile(int x, int y, float cycleTime, bool isAdvancing, TileType advancingType) 
        : base(TileType.Field, x, y, cycleTime, isAdvancing, advancingType)
        {}

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            this.SetSprite(TilesetSpriteManager.Instance.GetSprite(this.TileType));
        }
    }
}