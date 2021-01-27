using TheHarvest.FileManagers;

namespace TheHarvest.ECS.Components.Tiles
{
    public class Blueberry1Tile : Tile
    {
        public Blueberry1Tile(int x, int y, float cycleTime, bool isAdvancing, TileType advancingType) 
        : base(TileType.Blueberry1, x, y, cycleTime, isAdvancing, advancingType)
        {}

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            this.SetAnimation(TilesetSpriteManager.Instance.GetAnimation(this.TileType));
            this.PlayAnimation();
        }
    }
}