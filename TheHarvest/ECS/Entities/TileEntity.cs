using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;

using TheHarvest.ECS.Components;

namespace TheHarvest.ECS.Entities
{
    public class TileEntity : Entity
    {
        Tile tile;

        public TileEntity(Tile tile) : base(tile.Type + Utils.RandomString(8))
        {
            this.tile = tile;
            this.AddComponent(tile);
            this.AddComponent<SpriteAnimator>();
            this.UpdatePosition(tile.X, tile.Y);
        }

        public void SetPosition(int x, int y)
        {
            // +0.5 since entity position is based on center
            this.tile.X = x;
            this.tile.Y = y;
            this.UpdatePosition(x, y);
        }

        public void UpdatePosition(int x, int y)
        {
            this.SetPosition(new Vector2(x + 0.5f, y + 0.5f) * Tile.Size);
        }
    }
}