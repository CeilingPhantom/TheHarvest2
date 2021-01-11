using Nez;

using TheHarvest.ECS.Entities;
using TheHarvest.Util;

namespace TheHarvest.ECS.Components
{
    public class Farm : Component
    {
        public BoundlessSparseMatrix<TileEntity> Grid { get; } = new BoundlessSparseMatrix<TileEntity>();
        PlayerState playerState;

        FastList<TileEntity> initTileEntities = new FastList<TileEntity>();

        public string Name { get; }

        public Farm(string name) : base()
        {
            this.Name = name;
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            this.playerState = this.Entity.GetComponent<PlayerState>();
            if (this.initTileEntities.Length == 0)
                this.DefaultInitTileEntities();
            this.AttachTileEntitiesToScene();
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

        private void AttachTileEntitiesToScene()
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
    }
}