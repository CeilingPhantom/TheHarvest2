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
            var farm1 = new Farm("farm1");
            farm1.PlaceTile(new DirtTile(0, 0, 0, false, 0));
            SaveFileManager.Save("test1.dat", farm1);
        }
    }
}