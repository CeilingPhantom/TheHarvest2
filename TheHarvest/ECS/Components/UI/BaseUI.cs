using Nez;

using TheHarvest.Scenes;
using TheHarvest.Util.Input;

namespace TheHarvest.ECS.Components.UI
{
    public abstract class BaseUI : UICanvas, IInputable
    {
        public static readonly int InputPriority = 99;

        public BaseUI() : base()
        {
            this.RenderLayer = FarmScene.UIRenderLayer;
            InputManager.Instance.Register(this, InputPriority);
        }

        #region IInputable

        public abstract bool InputCollision();

        #endregion
    }
}