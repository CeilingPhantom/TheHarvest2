using Nez.Sprites;

using TheHarvest.FileManagers;

namespace TheHarvest.ECS.Components.Tiles
{
    public class WeakTile : Tile
    {
        public WeakTile(TileType tiletype, int x, int y) 
        : base(tiletype, x, y)
        {}

        public override void OnAddedToEntity()
        {
            this.SpriteAnimator = this.Entity.GetComponent<SpriteAnimator>();
            this.SetSprite(TilesetSpriteManager.Instance.GetSprite(this.TileType));
        }

        public override void Update()
        {
            // do nothing
        }
    }
}