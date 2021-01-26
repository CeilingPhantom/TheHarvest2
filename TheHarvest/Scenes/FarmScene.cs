using Microsoft.Xna.Framework;
using System;
using Nez;
using Nez.UI;

using TheHarvest.ECS.Entities;
using TheHarvest.UI;

namespace TheHarvest.Scenes
{
    public class FarmScene : Scene
    {
        static readonly Lazy<FarmScene> lazy = new Lazy<FarmScene>(() => new FarmScene());
        public static FarmScene Instance => lazy.Value;

        public static readonly int UIRenderLayer = 999;

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

            var ui = this.AddEntity(new UIEntity());
            this.AddRenderer(new ScreenSpaceRenderer(100, UIRenderLayer));
            this.AddRenderer(new RenderLayerExcludeRenderer(0, UIRenderLayer));
        }

        
    }
}
