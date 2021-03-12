using System.Linq;

using TheHarvest.ECS.Components.Tiles;
using TheHarvest.ECS.Entities;
using TheHarvest.Events;
using TheHarvest.Util;

namespace TheHarvest.ECS.Components.Farm
{
    public abstract class Grid : EventSubscribingComponent
    {
        protected internal BoundlessSparseMatrix<TileEntity> TileGrid = new BoundlessSparseMatrix<TileEntity>();
        // TODO will buffs stack?
        protected internal BoundlessSparseMatrix<Structure.StructureBuffs> TileBuffsGrid = new BoundlessSparseMatrix<Structure.StructureBuffs>();

        #region Grid Accessors and Manipulation

        public virtual Tile GetTile(int x, int y)
        {
            if (this.TileGrid[x, y] == null)
            {
                return null;
            }
            return this.TileGrid[x, y].Tile;
        }
        
        public virtual Tile[] AllTiles()
        {
            return this.TileGrid.AllValues().Select(tileEntity => tileEntity.Tile).ToArray();
        }
        
        public abstract TileEntity AddTile(Tile tile, bool isInit=false);

        public abstract bool RemoveTile(int x, int y);

        public virtual Structure.StructureBuffs GetTileBuffs(int x, int y)
        {
            return this.TileBuffsGrid[x, y];
        }

        #endregion
    }
}