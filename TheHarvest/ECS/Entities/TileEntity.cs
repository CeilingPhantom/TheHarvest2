using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;

using TheHarvest.ECS.Components;

namespace TheHarvest.ECS.Entities
{
    public class TileEntity : Entity
    {
        Tile tile;

        public TileEntity(Tile tile) : base()
        {
            this.tile = tile;
            this.AddComponent(tile);
            this.AddComponent<SpriteAnimator>();
            UpdatePosition();
        }

        public void UpdatePosition()
        {
            this.SetPosition(new Vector2(this.tile.X + 0.5f, this.tile.Y + 0.5f) * Tile.Size);
        }
    }
}