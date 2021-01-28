using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Nez;

using TheHarvest.ECS.Components.Player;
using TheHarvest.ECS.Components.Tiles;
using TheHarvest.FileManagers;

namespace TheHarvest.ECS.Components.Farm
{
    public class FarmDefaultTiler : RenderableComponent
    {
        FarmGrid farm;
        
        public static readonly TileType DefaultTileType = TileType.Dirt;
        Texture2D defaultTileTexture;

        PlayerCamera playerCamera = PlayerCamera.Instance;
        public override RectangleF Bounds => this.playerCamera.Bounds;

        public FarmDefaultTiler(FarmGrid farm)
        {
            this.farm = farm;
            this.RenderLayer = 99;
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
                    batcher.Draw(this.defaultTileTexture, new Vector2(x, y) * Tile.Size);
                }
            }
        }
    }
}