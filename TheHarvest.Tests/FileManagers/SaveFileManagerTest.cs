using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TheHarvest.ECS.Components;
using TheHarvest.FileManagers;

namespace TheHarvest.Tests.FileManagers
{
    [TestClass]
    public class SaveFileManagerTest
    {
        [TestMethod]
        public void SaveAndLoadTest1()
        {
            var farm1In = new Farm();
            var tile1 = Tile.CreateTile(TileType.Dirt, 1, 2, 10);
            farm1In.PlaceTile(tile1);
            SaveFileManager.Save("test1.dat", farm1In);

            SaveFileManager.Load("test1.dat");
            var farm1Out = SaveFileManager.LoadedFarm;
            Assert.IsTrue(farm1Out.Grid.AllItems().Length == 1);
            Assert.IsTrue(farm1Out.Grid[1, 2] != null);
            Assert.IsTrue(farm1Out.Grid[1, 2].GetComponent<Tile>().CompareTo(tile1) == 0);
            Assert.IsTrue(farm1Out.Grid[0, 0] == null);
        }

        [TestMethod]
        public void SaveAndLoadTest2()
        {
            var bytes = new byte[PlayerState.ChunkSize];
            BitConverter.GetBytes(9999).CopyTo(bytes, 0);
            BitConverter.GetBytes(98765.4321f).CopyTo(bytes, 4);
            bytes[8] = 21;
            bytes[9] = 2;
            bytes[10] = 202;
            PlayerState.Instance.LoadFromBytes(bytes);
            var farm2In = new Farm();
            var tile1 = Tile.CreateTile(TileType.Dirt, 0, 0);
            farm2In.PlaceTile(tile1);
            var tile2 = Tile.CreateTile(TileType.Grass, 0, 1);
            farm2In.PlaceTile(tile2);
            var tile3 = Tile.CreateTile(TileType.Grass, 1, 0);
            farm2In.PlaceTile(tile3);
            var tile4 = Tile.CreateTile(TileType.Dirt, 1, 1);
            farm2In.PlaceTile(tile4);
            SaveFileManager.Save("test2.dat", farm2In);

            SaveFileManager.Load("test2.dat");
            Assert.IsTrue(PlayerState.Instance.Money == 9999);
            Assert.IsTrue(PlayerState.Instance.TimeOfDay == 98765.4321f);
            Assert.IsTrue(PlayerState.Instance.Day == 21);
            Assert.IsTrue(PlayerState.Instance.Season == 2);
            Assert.IsTrue(PlayerState.Instance.Year == 202);
            var farm2Out = SaveFileManager.LoadedFarm;
            Assert.IsTrue(farm2Out.Grid.AllItems().Length == 4);
            Assert.IsTrue(farm2Out.Grid[0, 0].GetComponent<Tile>().CompareTo(tile1) == 0);
            Assert.IsTrue(farm2Out.Grid[0, 1].GetComponent<Tile>().CompareTo(tile2) == 0);
            Assert.IsTrue(farm2Out.Grid[1, 0].GetComponent<Tile>().CompareTo(tile3) == 0);
            Assert.IsTrue(farm2Out.Grid[1, 1].GetComponent<Tile>().CompareTo(tile4) == 0);
        }
    }
}