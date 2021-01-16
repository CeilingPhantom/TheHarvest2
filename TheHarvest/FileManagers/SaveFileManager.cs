using System;
using System.IO;
using System.Collections.Generic;
using Nez;

using TheHarvest.ECS.Entities;
using TheHarvest.ECS.Components;
using TheHarvest.Scenes;

namespace TheHarvest.FileManagers
{
    public static class SaveFileManager
    {
        public static Farm LoadedFarm = null;

        public static void Load(string filename)
        {
            if (File.Exists(filename))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
                {
                    LoadPlayerState(reader);
                    LoadFarm(reader);
                }
            }
            if (LoadedFarm == null)
                LoadedFarm = new Farm();
        }

        static void LoadPlayerState(BinaryReader reader)
        {
            var chunk = reader.ReadBytes(PlayerState.ChunkSize);
            if (chunk.Length > 0)
                PlayerState.Instance.LoadFromBytes(chunk);
        }

        static void LoadFarm(BinaryReader reader)
        {
            LoadedFarm = new Farm();
            var chunk = reader.ReadBytes(Tile.ChunkSize);
            while (chunk.Length > 0)
            {
                LoadedFarm.PlaceTile(Tile.CreateTile(chunk));
                chunk = reader.ReadBytes(Tile.ChunkSize);
            }
        }

        public static void Save(string filename="farm.dat", Farm farm=null)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
            {
                writer.Write(PlayerState.Instance.ToBytes());
                // if not specified, get farm scene instance's farm
                if (farm == null)
                    farm = FarmScene.Instance.FindEntity("farm").GetComponent<Farm>();
                foreach (var tileEntity in farm.Grid.AllItems())
                    writer.Write(tileEntity.GetComponent<Tile>().ToBytes());
            }
        }
    }
}