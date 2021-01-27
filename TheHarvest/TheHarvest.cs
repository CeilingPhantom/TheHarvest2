﻿using System.Linq;
using Nez;

using TheHarvest.ECS.Components;
using TheHarvest.Events;
using TheHarvest.FileManagers;
using TheHarvest.Scenes;

namespace TheHarvest
{
    public class TheHarvest : Core
    {

        protected override void Initialize()
        {
            base.Initialize();
            this.Window.AllowUserResizing = false;

            SaveFileManager.Instance.Load("farm.dat");
            TilesetSpriteManager.Instance.Load();
            EventManager.Instance.Initialize();
            Scene = FarmScene.Instance;
        }
    }
}
