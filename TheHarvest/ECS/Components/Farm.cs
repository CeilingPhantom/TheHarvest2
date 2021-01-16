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

        FastList<TileEntity> defaultTileEntityPool = new FastList<TileEntity>();

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
            var bounds = this.Entity.Scene.Camera.Bounds;

            System.Diagnostics.Debug.WriteLine("topleft: {0}, {1}", Math.Floor(bounds.X / Tile.Size), Math.Floor(bounds.Y / Tile.Size));

            var topTileOverflow = Math.Abs(bounds.Y % Tile.Size);
            System.Diagnostics.Debug.WriteLine("height: {0}", Math.Ceiling((bounds.Height - topTileOverflow) / Tile.Size) + (topTileOverflow == 0 ? 0 : 1));

            var leftTileOverflow = Math.Abs(bounds.X % Tile.Size);
            System.Diagnostics.Debug.WriteLine("width: {0}", Math.Ceiling((bounds.Width -leftTileOverflow) / Tile.Size) + (leftTileOverflow == 0 ? 0 : 1));

            // handle events
        }

        public override void Render(Batcher batcher, Camera camera)
        {}

        #region Event Processing

        #endregion
    }
}