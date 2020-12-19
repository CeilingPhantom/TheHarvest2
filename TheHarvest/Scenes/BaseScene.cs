using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;
using Nez.Sprites;
using Nez.Textures;

using TheHarvest.Events;

namespace TheHarvest.Scenes
{
    public abstract class BaseScene : Scene
    {
        public readonly string Name;
        EventController controller = new EventController();
        public EventController Controller => controller;

        public BaseScene(string name) : base()
        {
            this.Name = name;
        }

        public override void Initialize()
        {
            base.Initialize();
            AddRenderer(new DefaultRenderer());
            SetDefaultDesignResolution(1280, 720, SceneResolutionPolicy.ShowAllPixelPerfect);
        }
    }
}