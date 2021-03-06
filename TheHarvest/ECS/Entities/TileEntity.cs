using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;

using TheHarvest.ECS.Components.Tiles;

namespace TheHarvest.ECS.Entities
{
    public class TileEntity : Entity
    {
        public Tile Tile { get; private set; }

        public TileEntity(Tile tile) : base(tile.TileType + Utils.RandomString())
        {
            this.Tile = tile;
            this.AddComponent(tile);
            this.AddComponent<SpriteAnimator>();
            this.UpdatePosition(tile.X, tile.Y);
        }

        public void SetPosition(int x, int y)
        {
            this.Tile.X = x;
            this.Tile.Y = y;
            this.UpdatePosition(x, y);
        }

        public void UpdatePosition(int x, int y)
        {
            // +0.5 since entity position is based around center
            this.SetPosition(new Vector2(x + 0.5f, y + 0.5f) * Tile.SpriteSize);
        }
    }
}