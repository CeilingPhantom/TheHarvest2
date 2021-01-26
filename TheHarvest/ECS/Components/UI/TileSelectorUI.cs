using Microsoft.Xna.Framework;
using System.Text.RegularExpressions;
using Nez;
using Nez.UI;

using TheHarvest.ECS.Components;
using TheHarvest.Events;
using TheHarvest.FileManagers;
using TheHarvest.Scenes;
using TheHarvest.UI;

namespace TheHarvest.ECS.Components
{
    public class TileSelectorUI : UICanvas
    {
        TileWindow window;

        public TileSelectorUI() : base()
        {
            this.RenderLayer = FarmScene.UIRenderLayer;
            this.window = this.Stage.AddElement(new TileWindow());
            this.AddTileSelections();
        }

        void AddTileSelections()
        {
            this.AddTileSelection(TileType.Blueberry1);
            this.AddTileSelection(TileType.Carrot1);
            this.AddTileSelection(TileType.Potato1);
            this.AddTileSelection(TileType.Strawberry1);
            this.AddTileSelection(TileType.Wheat1);

            // important!
            this.window.Pack();
        }

        void AddTileSelection(TileType tileType)
        {
            this.window.Add(CreateTileSelectionButton(tileType)).SetExpandX().SetFillX();
            this.window.Row();
        }

        TileSelectionButton CreateTileSelectionButton(TileType tileType)
        {
            var style = new ImageTextButtonStyle();
            style.ImageUp = new SpriteDrawable(TilesetSpriteManager.Instance.GetSprite(tileType));
            var btn = new TileSelectionButton(tileType, style);
            btn.OnClicked += b => System.Diagnostics.Debug.WriteLine(((TileSelectionButton) b).TileType);
            return btn;
        }

        void BuyTile(Button b)
        {
            var p = PlayerCamera.Instance.MouseToTilePosition();
            EventManager.Instance.Publish(new AddTileEvent(Tile.CreateTile(TileType.Grass, (int) p.X, (int) p.Y)));
        }
    }

    internal class TileSelectionButton : ImageTextButton
    {
        static readonly int padY = 5;
        internal TileType TileType { get; private set; }

        internal TileSelectionButton(TileType tileType, ImageTextButtonStyle style) : 
            base(Regex.Match(tileType.ToString(), @"[a-zA-z]+").Value, style)
        {
            this.TileType = tileType;
            this.Left().PadTop(TileSelectionButton.padY).PadBottom(TileSelectionButton.padY);
        }
    }
}