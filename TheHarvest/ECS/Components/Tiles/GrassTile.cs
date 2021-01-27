using TheHarvest.FileManagers;

namespace TheHarvest.ECS.Components.Tiles
{
    public class GrassTile : Tile
    {
        public GrassTile(int x, int y, float cycleTime, bool isAdvancing, TileType advancingType) 
        : base(TileType.Grass, x, y, cycleTime, isAdvancing, advancingType)
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

            if (this.CycleTime >= 5)
                this.AdvanceTile();
            
        }
    }
}