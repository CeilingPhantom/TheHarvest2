using Microsoft.Xna.Framework;
using Nez;

using TheHarvest.ECS.Components.Player;
using TheHarvest.ECS.Components.Tiles;

namespace TheHarvest.ECS.Components.Farm
{
    public class TileHighlighter : RenderableComponent
    {
        PlayerCamera playerCamera = PlayerCamera.Instance;

        public TileHighlighter() : base()
        {
            this.RenderLayer = -1;
            this.Enabled = false;
        }

        public override RectangleF Bounds => this.playerCamera.Bounds;

        public override void Render(Batcher batcher, Camera camera)
        {
            var p = this.playerCamera.MouseToTilePosition();
            batcher.DrawHollowRect(p * Tile.SpriteSize, Tile.SpriteSize, Tile.SpriteSize, Color.Red, 2 / (1.3f + this.playerCamera.Camera.Zoom));
        }
    }
}