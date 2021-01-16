using Nez.Textures;

namespace TheHarvest.ECS.Components
{
    public class DirtTile : Tile
    {
        public static readonly string TexturePath = "imgs/tiles/dirt0";

        public DirtTile(int x, int y, float cycleTime, bool isAdvancing, TileType advancingType) 
        : base(TileType.Dirt, x, y, cycleTime, isAdvancing, advancingType)
        {}

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            this.SpriteAnimator.SetSprite(new Sprite(this.Entity.Scene.Content.LoadTexture(DirtTile.TexturePath)));
        }
    }
}