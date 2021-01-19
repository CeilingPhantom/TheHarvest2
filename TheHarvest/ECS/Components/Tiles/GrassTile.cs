using Nez.Textures;

namespace TheHarvest.ECS.Components
{
    public class GrassTile : Tile
    {
        public static readonly string TexturePath = "imgs/tiles/grass0_0";

        public GrassTile(int x, int y, float cycleTime, bool isAdvancing, TileType advancingType) 
        : base(TileType.Grass, x, y, cycleTime, isAdvancing, advancingType)
        {}

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            this.SpriteAnimator.SetSprite(new Sprite(this.Entity.Scene.Content.LoadTexture(GrassTile.TexturePath)));
        }
    }
}