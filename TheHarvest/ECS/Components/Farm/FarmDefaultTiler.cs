using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Nez;

using TheHarvest.FileManagers;

namespace TheHarvest.ECS.Components
{
    public class FarmDefaultTiler : RenderableComponent
    {
        Farm farm;
        
        public static readonly TileType DefaultTileType = TileType.Dirt;
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
            this.defaultTileTexture = TilesetSpriteManager.Instance.GetSprite(FarmDefaultTiler.DefaultTileType).Texture2D;
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