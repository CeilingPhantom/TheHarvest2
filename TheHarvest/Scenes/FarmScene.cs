using System;
using Nez;

using TheHarvest.ECS.Entities;

namespace TheHarvest.Scenes
{
    public class FarmScene : Scene
    {
        static readonly Lazy<FarmScene> lazy = new Lazy<FarmScene>(() => new FarmScene());
        public static FarmScene Instance => lazy.Value;

        private FarmScene() : base()
        {}

        public override void Initialize()
        {
            base.Initialize();
            this.AddRenderer(new DefaultRenderer());
            Scene.SetDefaultDesignResolution(1280, 720, SceneResolutionPolicy.ShowAllPixelPerfect);

            // setup camera
            var playerCamera = this.AddEntity(new PlayerEntity());
            this.Camera.AddComponent(new FollowCamera(playerCamera)).FollowLerp = 0.7f;
            
            this.AddEntity(new FarmEntity());
        }
    }
}
