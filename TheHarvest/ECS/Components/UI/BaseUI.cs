using Nez;

using TheHarvest.Scenes;
using TheHarvest.Util.Input;

namespace TheHarvest.ECS.Components.UI
{
    public abstract class BaseUI : UICanvas, IInputable
    {
        public BaseUI() : base()
        {
            this.RenderLayer = FarmScene.UIRenderLayer;
            // remember to register subclasses to input manager
        }

        #region IInputable

        public abstract bool InputCollision();

        #endregion
    }
}