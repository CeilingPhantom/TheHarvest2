using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;

using TheHarvest.FileManagers;
using TheHarvest.Scenes;
using TheHarvest.UI;

namespace TheHarvest.ECS.Components
{
    public class TileUI : UICanvas
    {
        public TileUI() : base()
        {
            this.RenderLayer = FarmScene.UIRenderLayer;
            var window = this.Stage.AddElement(new TileWindow()).Top().Left();
            var i = new ImageTextButtonStyle();
            i.ImageUp = new SpriteDrawable(TilesetSpriteManager.Instance.GetSprite(TileType.Grass));
            var b = new ImageTextButton("qwerty", i);
            b.OnClicked += x => System.Diagnostics.Debug.WriteLine("hello");
            window.Add(b).SetMinWidth(window.GetWidth()).SetMinHeight(50);
        }
    }
}