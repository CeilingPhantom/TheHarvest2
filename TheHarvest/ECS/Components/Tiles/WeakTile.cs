using Nez.Sprites;

using TheHarvest.FileManagers;

namespace TheHarvest.ECS.Components.Tiles
{
    public class WeakTile : Tile
    {
        public WeakTile(TileType tiletype, int x, int y) 
        : base(tiletype, x, y)
        {}

        public WeakTile(Tile tile)
        : base(tile.TileType, tile.X, tile.Y)
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