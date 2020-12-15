using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;
using Nez.Sprites;
using Nez.Textures;

namespace TheHarvest.Scenes
{
    public class BaseScene : Scene
    {
        public BaseScene() : base() { }

        public override void Initialize()
        {
            base.Initialize();
            AddRenderer(new DefaultRenderer());
            SetDefaultDesignResolution(1280, 720, SceneResolutionPolicy.ShowAllPixelPerfect);
        }
    }
}