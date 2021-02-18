using Nez.UI;

using TheHarvest.ECS.Components.Player;
using TheHarvest.ECS.Components.Tiles;
using TheHarvest.Events;
using TheHarvest.FileManagers;
using TheHarvest.UI;
using TheHarvest.Util.Input;

namespace TheHarvest.ECS.Components.UI
{
    public class TileSelectorUI : BaseUI
    {
        BaseWindow window;

        static readonly int inputPriority = 91;

        public TileSelectorUI() : base()
        {
            InputManager.Instance.Register(this, TileSelectorUI.inputPriority);

            this.window = this.Stage.AddElement(new BaseWindow());
            this.SetEditButton();

            this.window.SetPosition(100, 100);
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
            this.AddTileSelection(TileType.Destruct);
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

            var buttonsSubTable = new Table();
            this.window.Add(buttonsSubTable).GrowX();
            var applyButtonCell = buttonsSubTable.Add(applyButton).SetPrefHeight(buttonsSubTable.GetWidth() / 2).GrowX().Center();
            var cancelButtonCell = buttonsSubTable.Add(cancelButton).SetPrefHeight(buttonsSubTable.GetWidth() / 2).GrowX().Center();
            buttonsSubTable.Pack();
            applyButtonCell.SetElementWidth(buttonsSubTable.GetWidth() / 2);
            cancelButtonCell.SetElementWidth(buttonsSubTable.GetWidth() / 2);
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

        public override void Update()
        {
            if (InputManager.Instance.CanAcceptInput(TileSelectorUI.inputPriority))
            {
                base.Update();
            }
        }

        #region IInputable

        public override bool InputCollision()
        {
            var mousePos = this.Stage.GetMousePosition();
            if (mousePos.X >= this.window.GetX() && 
                mousePos.Y >= this.window.GetY() && 
                mousePos.X < this.window.GetX() + this.window.GetWidth() && 
                mousePos.Y < this.window.GetY() + this.window.GetHeight() 
                || 
                this.window.IsDragging())
            {
                return true;
            }
            return false;
        }

        #endregion
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
            this.GetImage().SetScale(2);
            this.Pack();
        }
    }
}