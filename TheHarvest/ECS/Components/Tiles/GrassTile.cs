using TheHarvest.FileManagers;

namespace TheHarvest.ECS.Components.Tiles
{
    public class GrassTile : Tile
    {
        public GrassTile(int x, int y, bool isAdvancing, TileType advancingType, float cycleTime) 
        : base(TileType.Grass, x, y, 0, isAdvancing, advancingType, cycleTime)
        {}

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            this.SetAnimation(TilesetSpriteManager.Instance.GetAnimation(this.TileType));
            this.PlayAnimation();

            this.AdvancingType = TileType.Dirt;
            
        }

        public override void Update()
        {
            base.Update();

            if (this.CycleTime >= 4)
                this.AdvanceTile();
            
        }
    }
}