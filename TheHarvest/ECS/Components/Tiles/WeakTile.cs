using Nez.Sprites;
using TheHarvest.ECS.Components.Farm;
using TheHarvest.FileManagers;

namespace TheHarvest.ECS.Components.Tiles
{
    public class WeakTile : Tile
    {
        public WeakTile(TileType tiletype, int x, int y, Grid grid) 
        : base(tiletype, x, y, grid)
        {}

        public WeakTile(Tile tile)
        : base(tile.TileType, tile.X, tile.Y, tile.Grid)
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