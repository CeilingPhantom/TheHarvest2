using Nez;

using TheHarvest.ECS.Entities;
using TheHarvest.Events;

namespace TheHarvest.Scenes
{
    public abstract class BaseScene : Scene
    {
        EventController controller = new EventController();
        public EventController Controller => controller;

        public BaseScene() : base()
        {}

        public override void Initialize()
        {
            base.Initialize();
            AddRenderer(new DefaultRenderer());
            SetDefaultDesignResolution(1280, 720, SceneResolutionPolicy.ShowAllPixelPerfect);
            var playerCamera = this.AddEntity(new PlayerCameraEntity());
            this.Camera.AddComponent(new FollowCamera(playerCamera));
        }
    }
}