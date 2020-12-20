using System;
using System.IO;
using System.Collections.Generic;
using Nez;

using TheHarvest.ECS.Entities;
using TheHarvest.ECS.Components;
using TheHarvest.Util;

namespace TheHarvest.FileManagers
{
    public static class SaveFileManager
    {
        // magic number/chunk to separate different grids is a tile with the same position as the last
        // works since there can't be two tiles in the same position in a grid

        public static Dictionary<string, Farm> LoadedFarms { get; } = new Dictionary<string, Farm>();

        public static void Load(string filename)
        {
            if (File.Exists(filename))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
                {
                    LoadPlayerState(reader);
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                        LoadFarm(reader);
                }
            }
        }

        private static void LoadPlayerState(BinaryReader reader)
        {
            var chunk = reader.ReadBytes(PlayerState.ChunkSize);
            if (chunk.Length > 0)
                PlayerState.Instance.LoadFromBytes(chunk);
        }

        private static void LoadFarm(BinaryReader reader)
        {
            var name = reader.ReadString();
            var farm = new Farm(name);
            var chunk = reader.ReadBytes(Tile.ChunkSize);
            Tile prevTile = null;
            while (chunk.Length > 0)
            {
                var tile = Tile.CreateTile(chunk);
                // first chunk
                if (prevTile == null)
                    prevTile = tile;
                // check if current tile has the same (x, y) as the prev one
                // if the grid has 1 tile, its save will have 2 tiles
                // if the grid has 0 tiles, its save will have 0 tiles
                else if (prevTile.X == tile.X && prevTile.Y == tile.Y)
                    // we've reached the end for this grid
                    break;
                farm.PlaceTile(tile);
                chunk = reader.ReadBytes(Tile.ChunkSize);
            }
            LoadedFarms[name] = farm;
        }

        public static Farm GetLoadedFarm(string name)
        {
            Farm val;
            if (LoadedFarms.TryGetValue(name, out val))
                return val;
            return null;
        }

        public static void Save(string filename, params Farm[] farms)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
            {
                writer.Write(PlayerState.Instance.ToBytes());
                if (farms.Length == 0)
                {
                    Array.Resize(ref farms, TheHarvest.Scenes.Count);
                    int i = 0;
                    foreach (var sceneEntry in TheHarvest.Scenes)
                        farms[i++] = sceneEntry.Value().EntitiesOfType<FarmEntity>()[0].GetComponent<Farm>();
                }
                byte[] farmSeparator = null;
                foreach (var farm in farms)
                {
                    if (farmSeparator != null)
                        writer.Write(farmSeparator);
                    writer.Write(farm.Name);
                    var tileEntities = farm.Grid.AllItems();
                    foreach (var tileEntity in tileEntities)
                        writer.Write(tileEntity.GetComponent<Tile>().ToBytes());
                    // write dup of last tile
                    farmSeparator = tileEntities[tileEntities.Length - 1].GetComponent<Tile>().ToBytes();
                }
            }
        }
    }
}