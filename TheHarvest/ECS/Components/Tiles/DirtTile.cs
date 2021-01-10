using Nez;
using Nez.Textures;

namespace TheHarvest.ECS.Components
{
    public class DirtTile : Tile
    {
        public DirtTile(int x, int y, float cycleTime, bool isAdvancing, TileType advancingType) 
        : base(TileType.Dirt, x, y, cycleTime, isAdvancing, advancingType)
        {}

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            this.SpriteRenderer.SetSprite(new Sprite(this.Entity.Scene.Content.LoadTexture("Content/imgs/tiles/dirt0.png")));
        }
    }
}