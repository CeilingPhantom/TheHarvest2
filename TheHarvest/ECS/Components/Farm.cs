using System.IO;
using Nez;

using TheHarvest.ECS.Entities;
using TheHarvest.Util;

namespace TheHarvest.ECS.Components
{
    public class Farm : Component
    {
        public BoundlessSparseMatrix<TileEntity> Grid { get; } = new BoundlessSparseMatrix<TileEntity>();
        PlayerState playerState;

        FastList<TileEntity> init_tile_entities = new FastList<TileEntity>();

        public string Name { get; }

        public Farm(string name) : base()
        {
            this.Name = name;
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            this.playerState = this.Entity.GetComponent<PlayerState>();
            for (var i = 0; i < this.init_tile_entities.Length; ++i)
                this.Entity.Scene.AddEntity(this.init_tile_entities[i]);
            this.init_tile_entities.Clear();
        }

        public TileEntity PlaceTile(Tile tile) {
            tile.Farm = this;
            var tileEntity = new TileEntity(tile);
            this.Grid[tile.X, tile.Y] = tileEntity;
            if (this.Entity != null && this.Entity.Scene != null)
                this.Entity.Scene.AddEntity(tileEntity);
            else
                this.init_tile_entities.Add(tileEntity);
            return this.Grid[tile.X, tile.Y];
        }

        // public void EnableTileRender()
        // public void DisableTileRender()
    }
}