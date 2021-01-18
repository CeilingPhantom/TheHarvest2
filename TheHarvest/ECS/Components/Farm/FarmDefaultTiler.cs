using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Nez;

namespace TheHarvest.ECS.Components
{
    public class FarmDefaultTiler : RenderableComponent
    {
        Farm farm;
        
        static string defaultTileType = DirtTile.TexturePath;
        Texture2D defaultTileTexture;

        public override RectangleF Bounds => this.Entity.Scene.Camera.Bounds;

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
            var topLeftX = (int) Math.Floor(this.Bounds.X / Tile.Size);
            var topLeftY = (int) Math.Floor(this.Bounds.Y / Tile.Size);
            var topTileOverflow = Math.Abs(this.Bounds.Y % Tile.Size);
            var leftTileOverflow = Math.Abs(this.Bounds.X % Tile.Size);
            var width = Math.Ceiling((this.Bounds.Width -leftTileOverflow) / Tile.Size) + (leftTileOverflow == 0 ? 0 : 1);
            var height = Math.Ceiling((this.Bounds.Height - topTileOverflow) / Tile.Size) + (topTileOverflow == 0 ? 0 : 1);
            for (var x = topLeftX; x < topLeftX + width; ++x)
            {
                for (var y = topLeftY; y < topLeftY + height; ++y)
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