using Nez;

using TheHarvest.Scenes;

namespace TheHarvest
{
    public class TheHarvest : Core
    {
        protected override void Initialize()
        {
            base.Initialize();
            Window.AllowUserResizing = true;
            Scene = FarmScene.Instance;
        }
    }
}
