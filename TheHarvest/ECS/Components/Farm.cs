using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Nez;

using TheHarvest.ECS.Entities;
using TheHarvest.Events;
using TheHarvest.Util;

namespace TheHarvest.ECS.Components
{
    public class Farm : RenderableEventSubscriberComponent
    {
        public BoundlessSparseMatrix<TileEntity> Grid { get; } = new BoundlessSparseMatrix<TileEntity>();
        PlayerState playerState = PlayerState.Instance;

        string defaultTile = DirtTile.TexturePath;
        Texture2D defaultTileTexture;

        FastList<TileEntity> initTileEntities = new FastList<TileEntity>();

        public override float Width => Grid.Width;
        public override float Height => Grid.Height;

        public Farm() : base()
        {}

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            //if (this.initTileEntities.Length == 0)
            //    this.DefaultInitTileEntities();
            this.AttachInitTileEntitiesToScene();
            this.defaultTileTexture = this.Entity.Scene.Content.LoadTexture(this.defaultTile);
        }

        private void DefaultInitTileEntities()
        {
            // TODO more elaborate default tiling
            var w = 20;
            var h = 12;
            for (var i = 0; i < w; ++i)
                for (var j = 0; j < h; ++j)
                    this.PlaceTile(Tile.CreateTile(TileType.Dirt, i, j));
        }

        private void AttachInitTileEntitiesToScene()
        {
            for (var i = 0; i < this.initTileEntities.Length; ++i)
                this.Entity.Scene.AddEntity(this.initTileEntities[i]);
            this.initTileEntities.Clear();
        }

        public TileEntity PlaceTile(Tile tile)
        {
            tile.Farm = this;
            var tileEntity = new TileEntity(tile);
            this.Grid[tile.X, tile.Y] = tileEntity;
            if (this.Entity != null && this.Entity.Scene != null)
                this.Entity.Scene.AddEntity(tileEntity);
            else
                this.initTileEntities.Add(tileEntity);
            return this.Grid[tile.X, tile.Y];
        }

        // public void EnableTileRender()
        // public void DisableTileRender()

        public override void Update()
        {
            // handle events
        }

        // to ensure that default tiles are always rendered no matter the size of the grid
        public override bool IsVisibleFromCamera(Camera camera)
        {
            return true;
        }

        public override void Render(Batcher batcher, Camera camera)
        {
            var bounds = this.Entity.Scene.Camera.Bounds;
            var topLeftX = (int) Math.Floor(bounds.X / Tile.Size);
            var topLeftY = (int) Math.Floor(bounds.Y / Tile.Size);
            var topTileOverflow = Math.Abs(bounds.Y % Tile.Size);
            var leftTileOverflow = Math.Abs(bounds.X % Tile.Size);
            var width = Math.Ceiling((bounds.Width -leftTileOverflow) / Tile.Size) + (leftTileOverflow == 0 ? 0 : 1);
            var height = Math.Ceiling((bounds.Height - topTileOverflow) / Tile.Size) + (topTileOverflow == 0 ? 0 : 1);
            System.Diagnostics.Debug.WriteLine("topleft: {0}, {1}", Math.Floor(bounds.X / Tile.Size), Math.Floor(bounds.Y / Tile.Size));
            System.Diagnostics.Debug.WriteLine("width: {0}", Math.Ceiling((bounds.Width -leftTileOverflow) / Tile.Size) + (leftTileOverflow == 0 ? 0 : 1));
            System.Diagnostics.Debug.WriteLine("height: {0}", Math.Ceiling((bounds.Height - topTileOverflow) / Tile.Size) + (topTileOverflow == 0 ? 0 : 1));

            for (var x = topLeftX; x < topLeftX + width; ++x)
            {
                for (var y = topLeftY; y < topLeftY + height; ++y)
                {
                    if (Grid[x, y] == null)
                    {
                        batcher.Draw(this.defaultTileTexture, new Vector2(x, y) * Tile.Size);
                    }
                }
            }
        }

        #region Event Processing

        #endregion
    }
}