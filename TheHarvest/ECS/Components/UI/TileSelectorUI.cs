using System.Text.RegularExpressions;
using Nez;
using Nez.UI;

using TheHarvest.ECS.Components.Player;
using TheHarvest.ECS.Components.Tiles;
using TheHarvest.Events;
using TheHarvest.FileManagers;
using TheHarvest.Scenes;
using TheHarvest.UI;

namespace TheHarvest.ECS.Components.UI
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
            var button = new TileSelectionButton(tileType, style);
            button.OnClicked += this.SelectTile;
            return button;
        }

        void SelectTile(Button button)
        {
            System.Diagnostics.Debug.WriteLine(((TileSelectionButton) button).TileType);
            var p = PlayerCamera.Instance.MouseToTilePosition();
            EventManager.Instance.Publish(new TileSelectionEvent(((TileSelectionButton) button).TileType));
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