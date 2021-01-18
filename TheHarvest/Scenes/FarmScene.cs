using Microsoft.Xna.Framework;
using System;
using Nez;
using Nez.UI;

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

            var ui = this.CreateEntity("ui");
            var canvas = ui.AddComponent<UICanvas>();
            canvas.RenderLayer = 999;
            var window = canvas.Stage.AddElement(new Window("hello world", new WindowStyle()));
            //window.SetBackground(new PrimitiveDrawable(Color.Black));
            AddRenderer(new ScreenSpaceRenderer(100, 999));
            AddRenderer(new RenderLayerExcludeRenderer(0, 999));
            window.DebugAll();
            window.SetPosition(100, 100);
            window.PadTop(10);
            //window.AddElement(new Slider( 0, 1, 0.1f, false, SliderStyle.Create( Color.DarkGray, Color.LightYellow ) ));
        }

        
    }
}
