using System.IO;
using Nez;

using TheHarvest.ECS.Entities;
using TheHarvest.FileManagers;
using TheHarvest.Util;

namespace TheHarvest.ECS.Components
{
    public class Farm : Component
    {
        public BoundlessSparseMatrix<TileEntity> Grid { get; }

        // PlayerState

        public Farm(string filename)
        {
            SaveFileManager.Load(this, filename);
            this.Grid = SaveFileManager.Grid;
        }

        // public void AddTile(Tile tile)
    }
}