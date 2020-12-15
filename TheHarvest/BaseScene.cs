using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;
using Nez.Sprites;
using Nez.Textures;

namespace TheHarvest.Scenes {
    public class BaseScene : Scene {
        public BaseScene() : base() { }

        public override void Initialize()
        {
            base.Initialize();
            AddRenderer(new DefaultRenderer());
            SetDefaultDesignResolution(1280, 720, SceneResolutionPolicy.ShowAllPixelPerfect);

            var bgEntity = CreateEntity("bg");
            var ui = bgEntity.AddComponent<UICanvas>();

            var table = ui.Stage.AddElement( new Table() );
            var dirtSprite = new Sprite(Content.LoadTexture("imgs/tiles/dirt0"));
            var bg = new ScaledTiledDrawable(dirtSprite);
            bg.SetScale(4f);
            table.SetBackground(bg);

            // tell the table to fill all the available space. In this case that would be the entire screen.
            table.SetFillParent( true );

            // add a ProgressBar
            var bar = new ProgressBar( 0, 1, 0.1f, false, ProgressBarStyle.Create( Color.Black, Color.White ) );
            table.Add( bar );

            // this tells the table to move on to the next row
            table.Row();

            // add a Slider
            var slider = new Slider( 0, 1, 0.1f, false, SliderStyle.Create( Color.DarkGray, Color.LightYellow ) );
            table.Add( slider );
            table.Row();

            // if creating buttons with just colors (PrimitiveDrawables) it is important to explicitly set the minimum size since the colored textures created
            // are only 1x1 pixels
            var button = new Button( ButtonStyle.Create( Color.Black, Color.DarkGray, Color.Green ) );
            table.Add( button ).SetMinWidth( 100 ).SetMinHeight( 30 );

            var grassEntity = CreateEntity("grass");
            grassEntity.AddComponent(new SpriteRenderer(Content.LoadTexture("imgs/tiles/grass0_0")));
        }
    }
}