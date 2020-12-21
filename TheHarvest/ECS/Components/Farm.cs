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

        public string Name { get; }

        public Farm(string name) : base()
        {
            this.Name = name;
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            playerState = this.Entity.GetComponent<PlayerState>();
        }

        public TileEntity PlaceTile(Tile tile) {
            tile.Farm = this;
            Grid[tile.X, tile.Y] = new TileEntity(tile);
            return Grid[tile.X, tile.Y];
        }

        // public void EnableTileRender()
        // public void DisableTileRender()
    }
}