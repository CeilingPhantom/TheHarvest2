using Microsoft.Xna.Framework.Input;
using Nez;

using TheHarvest.ECS.Entities;
using TheHarvest.Events;
using TheHarvest.Util;

namespace TheHarvest.ECS.Components
{
    public class Farm : EventSubscribingUpdatableComponent
    {
        public BoundlessSparseMatrix<TileEntity> Grid { get; } = new BoundlessSparseMatrix<TileEntity>();
        PlayerState playerState = PlayerState.Instance;
        PlayerCamera playerCamera = PlayerCamera.Instance;

        FastList<TileEntity> initTileEntities = new FastList<TileEntity>();

        public Farm() : base()
        {}

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            
            this.AddTile(Tile.CreateTile(TileType.Grass, 0, 0));
            
            this.AttachInitTileEntitiesToScene();
        }

        private void AttachInitTileEntitiesToScene()
        {
            for (var i = 0; i < this.initTileEntities.Length; ++i)
                this.Entity.Scene.AddEntity(this.initTileEntities[i]);
            this.initTileEntities.Clear();
        }

        public TileEntity AddTile(Tile tile)
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

        public void RemoveTile(Tile tile)
        {
            this.Grid.Remove(tile.X, tile.Y);
        }

        // public void EnableTileRender()
        // public void DisableTileRender()

        public override void Update()
        {
            // handle events
            base.Update();
        }

        #region Event Processing

        #endregion
    }
}