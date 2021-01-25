using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Nez;

namespace TheHarvest.ECS.Components
{
    public class FarmDefaultTiler : RenderableComponent
    {
        Farm farm;
        
        static string defaultTileType = "imgs/tiles/dirt0";
        Texture2D defaultTileTexture;

        PlayerCamera playerCamera = PlayerCamera.Instance;
        public override RectangleF Bounds => this.playerCamera.Bounds;

        public FarmDefaultTiler(Farm farm)
        {
            this.farm = farm;
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            this.defaultTileTexture = this.Entity.Scene.Content.LoadTexture(FarmDefaultTiler.defaultTileType);
        }

        public override void Render(Batcher batcher, Camera camera)
        {
            for (var x = this.playerCamera.TopLeftTileX; x < this.playerCamera.TopLeftTileX + this.playerCamera.WidthTiles; ++x)
            {
                for (var y = this.playerCamera.TopLeftTileY; y < this.playerCamera.TopLeftTileY + this.playerCamera.HeightTiles; ++y)
                {
                    if (this.farm.Grid[x, y] == null)
                    {
                        batcher.Draw(this.defaultTileTexture, new Vector2(x, y) * Tile.Size);
                    }
                }
            }
        }
    }
}