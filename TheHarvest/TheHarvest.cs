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
            EventManager.Instance.Initialize();
            Scene = FarmScene.Instance;
        }
    }
}
