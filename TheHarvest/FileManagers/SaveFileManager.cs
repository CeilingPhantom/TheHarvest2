using System;
using System.IO;
using System.Collections.Generic;
using Nez;

using TheHarvest.ECS.Entities;
using TheHarvest.ECS.Components;
using TheHarvest.Scenes;

namespace TheHarvest.FileManagers
{
    public class SaveFileManager
    {
        static readonly Lazy<SaveFileManager> lazy = new Lazy<SaveFileManager>(() => new SaveFileManager());
        public static SaveFileManager Instance => lazy.Value;

        private SaveFileManager()
        {}

        public Farm LoadedFarm = null;

        public void Load(string filename)
        {
            if (File.Exists(filename))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
                {
                    LoadPlayerState(reader);
                    LoadFarm(reader);
                }
            }
            if (this.LoadedFarm == null)
                this.LoadedFarm = new Farm();
        }

        void LoadPlayerState(BinaryReader reader)
        {
            var chunk = reader.ReadBytes(PlayerState.ChunkSize);
            if (chunk.Length > 0)
                PlayerState.Instance.LoadFromBytes(chunk);
        }

        void LoadFarm(BinaryReader reader)
        {
            this.LoadedFarm = new Farm();
            var chunk = reader.ReadBytes(Tile.ChunkSize);
            while (chunk.Length > 0)
            {
                this.LoadedFarm.AddTile(Tile.CreateTile(chunk));
                chunk = reader.ReadBytes(Tile.ChunkSize);
            }
        }

        public void Save(string filename="farm.dat", Farm farm=null)
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