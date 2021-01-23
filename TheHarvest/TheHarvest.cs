using System.Linq;
using Nez;

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
            Window.AllowUserResizing = false;

            SaveFileManager.Load("farm.dat");
            TmxTilesetTileManager.Load();
            TmxTilesetTileManager.tiletypeToTileId.Select(i => $"{i.Key}: {i.Value}").ToList().ForEach(s => System.Diagnostics.Debug.WriteLine(s));
            EventManager.Instance.Initialize();
            Scene = FarmScene.Instance;
        }
    }
}
