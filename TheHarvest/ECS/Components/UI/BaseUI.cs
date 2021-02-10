using Nez;

using TheHarvest.Scenes;

namespace TheHarvest.ECS.Components.UI
{
    public class BaseUI : UICanvas
    {
        public BaseUI() : base()
        {
            this.RenderLayer = FarmScene.UIRenderLayer;
        }
    }
}