using System;
using System.IO;

using TheHarvest.ECS.Entities;
using TheHarvest.ECS.Components;
using TheHarvest.Util;

namespace TheHarvest.FileManagers
{
    public static class SaveFileManager
    {
        public static BoundlessSparseMatrix<TileEntity> Grid { get; private set; }
        //public static PlayerState { get; private set; }
        //public static Settings

        public static void Load(Farm farm, string filename)
        {
            Grid = new BoundlessSparseMatrix<TileEntity>();
            if (File.Exists(filename))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
                {
                    var chunk = reader.ReadBytes(Tile.ChunkSize);
                    while (chunk.Length > 0)
                    {
                        var tile = Tile.CreateTile(farm, chunk);
                        Grid[tile.X, tile.Y] = new TileEntity(tile);
                        chunk = reader.ReadBytes(Tile.ChunkSize);
                    }
                }
            }
        }

        public static void Save(Farm farm, string filename)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.Truncate)))
            {
                foreach (var tileEntity in farm.Grid.AllItems())
                    writer.Write(tileEntity.GetComponent<Tile>().ToBytes());
            }
        }
    }
}