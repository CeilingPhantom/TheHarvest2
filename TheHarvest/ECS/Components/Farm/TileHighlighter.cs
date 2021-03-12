using Microsoft.Xna.Framework;
using Nez;

using TheHarvest.ECS.Components.Player;
using TheHarvest.ECS.Components.Tiles;

namespace TheHarvest.ECS.Components.Farm
{
    public class TileHighlighter : RenderableComponent
    {
        TentativeFarmGrid farm;
        PlayerCamera playerCamera = PlayerCamera.Instance;

        public TileHighlighter(TentativeFarmGrid farm) : base()
        {
            this.farm = farm;
            this.RenderLayer = -1;
            this.Enabled = false;
        }

        public override RectangleF Bounds => this.playerCamera.Bounds;

        public bool HighlightTileAtMouse = false;

        float hollowRectBorderThickness => 2 / (1.3f + this.playerCamera.Camera.Zoom);

        public override void Render(Batcher batcher, Camera camera)
        {
            foreach (var val in farm.InvalidTiles)
            {
                batcher.DrawHollowRect(new Vector2(val.X, val.Y) * Tile.SpriteSize, Tile.SpriteSize, Tile.SpriteSize, Color.Red, this.hollowRectBorderThickness);
            }

            if (this.HighlightTileAtMouse)
            {
                var p = this.playerCamera.MouseToTilePosition();
                batcher.DrawHollowRect(p * Tile.SpriteSize, Tile.SpriteSize, Tile.SpriteSize, Color.Green, this.hollowRectBorderThickness);
            }
        }
    }
}