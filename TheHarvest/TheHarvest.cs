using System;
using System.Collections.Generic;
using Nez;

using TheHarvest.Scenes;
using TheHarvest.FileManagers;

namespace TheHarvest
{
    public class TheHarvest : Core
    {
        public static readonly Dictionary<string, Func<Scene>> Scenes = new Dictionary<string, Func<Scene>>
        {
            {"farm1", () => Farm1Scene.Instance},
        };

        protected override void Initialize()
        {
            base.Initialize();
            Window.AllowUserResizing = true;
            SaveFileManager.Load("farm.dat");
            Scene = Scenes["farm1"]();
        }
    }
}
