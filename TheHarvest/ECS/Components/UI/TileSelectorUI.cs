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
            this.SetEditButton();
            // TODO toggle edit mode button
        }

        void SetTileSelections()
        {
            this.window.Clear();
            this.AddTileSelections();
            this.AddApplyCancelButtons();
            this.window.Pack();
        }

        void SetEditButton()
        {
            this.window.Clear();
            this.AddEditButton();
            this.window.Pack();
        }

        void AddTileSelections()
        {
            this.AddTileSelection(TileType.Blueberry1);
            this.AddTileSelection(TileType.Carrot1);
            this.AddTileSelection(TileType.Potato1);
            this.AddTileSelection(TileType.Strawberry1);
            this.AddTileSelection(TileType.Wheat1);
        }

        void AddTileSelection(TileType tileType)
        {
            this.window.Add(CreateTileSelectionButton(tileType)).GrowX();
            this.window.Row();
        }

        void AddEditButton()
        {
            var button = new TextButton("Edit", new TextButtonStyle());
            button.OnClicked += b => 
            {
                EventManager.Instance.Publish(new EditFarmOnEvent());
                this.SetTileSelections();
            };
            this.window.Add(button);
            this.window.Row();
        }

        void AddApplyCancelButtons()
        {
            var applyButton = new TextButton("Apply", new TextButtonStyle());
            applyButton.OnClicked += b => 
            {
                EventManager.Instance.Publish(new EditFarmOffEvent(true));
                this.SetEditButton();
            };
            var cancelButton = new TextButton("Cancel", new TextButtonStyle());
            cancelButton.OnClicked += b => 
            {
                EventManager.Instance.Publish(new EditFarmOffEvent(false));
                this.SetEditButton();
            };

            var t = new Table();
            t.DebugAll();
            this.window.Add(t).GrowX();
            var a = t.Add(applyButton).SetPrefHeight(t.GetWidth() / 2).GrowX().Center();
            var c = t.Add(cancelButton).SetPrefHeight(t.GetWidth() / 2).GrowX().Center();
            t.Pack();
            a.SetPrefHeight(t.GetWidth() / 2);
            c.SetPrefHeight(t.GetWidth() / 2);
            System.Diagnostics.Debug.WriteLine(a.GetElementWidth());
            System.Diagnostics.Debug.WriteLine(c.GetElementWidth());
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
            base(Tile.BaseTileType(tileType), style)
        {
            this.TileType = tileType;
            this.Left().PadTop(TileSelectionButton.padY).PadBottom(TileSelectionButton.padY);
            this.GetLabelCell().SetPadLeft(3);
            this.Pack();
        }
    }
}