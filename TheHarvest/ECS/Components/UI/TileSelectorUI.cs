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

            this.window.DebugAll();
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
            this.AddTileSelection(TileType.Upgrade);
            this.AddTileSelection(TileType.Blueberry1);
            this.window.Row();

            this.AddTileSelection(TileType.Destruct);
            this.AddTileSelection(TileType.Carrot1);
            this.window.Row();

            this.AddTileSelection(TileType.Reset);
            this.AddTileSelection(TileType.Potato1);
            this.window.Row();

            this.AddUndoButton();
            this.AddTileSelection(TileType.Strawberry1);
            this.window.Row();

            this.AddRedoButton();
            this.AddTileSelection(TileType.Wheat1);
            this.window.Row();
        }

        void AddTileSelection(TileType tileType)
        {
            this.window.Add(CreateTileSelectionButton(tileType)).GrowX();
        }

        void AddEditButton()
        {
            var button = new TextButton("Edit", new TextButtonStyle());
            button.OnClicked += b => 
            {
                EventManager.Instance.Publish(new TentativeFarmGridOnEvent());
                this.SetTileSelections();
                // TODO reset, undo, redo tile buttons
            };
            this.window.Add(button);
            this.window.Row();
        }

        void AddApplyCancelButtons()
        {
            var applyButton = new TextButton("Apply", new TextButtonStyle());
            applyButton.OnClicked += b => 
            {
                EventManager.Instance.Publish(new TentativeFarmGridOffEvent(true));
                this.SetEditButton();
            };
            var cancelButton = new TextButton("Cancel", new TextButtonStyle());
            cancelButton.OnClicked += b => 
            {
                EventManager.Instance.Publish(new TentativeFarmGridOffEvent(false));
                this.SetEditButton();
            };

            var buttonsSubTable = new Table();
            buttonsSubTable.PadTop(5).PadBottom(5);
            this.window.Add(buttonsSubTable).GrowX().SetColspan(this.window.GetColumns());
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
            EventManager.Instance.Publish(new TileSelectionEvent(((TileSelectionButton) button).TileType));
        }
        
        TileSelectionButton AddUndoButton()
        {
            var style = new ImageTextButtonStyle();
            style.ImageUp = new SpriteDrawable(TilesetSpriteManager.Instance.GetSprite("undo"));
            var button = new TileSelectionButton("Undo", style);
            button.OnClicked += this.Undo;
            this.window.Add(button).GrowX();
            return button;
        }

        void Undo(Button button)
        {
            EventManager.Instance.Publish(new TentativeFarmGridUndoEvent());
        }

        TileSelectionButton AddRedoButton()
        {
            var style = new ImageTextButtonStyle();
            style.ImageUp = new SpriteDrawable(TilesetSpriteManager.Instance.GetSprite("redo"));
            var button = new TileSelectionButton("Redo", style);
            button.OnClicked += this.Redo;
            this.window.Add(button).GrowX();
            return button;
        }

        void Redo(Button button)
        {
            EventManager.Instance.Publish(new TentativeFarmGridRedoEvent());
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
        static readonly int pad = 5;
        internal TileType TileType { get; private set; }

        internal TileSelectionButton(string tileName, ImageTextButtonStyle style) : 
            base(tileName, style)
        {
            this.Left().Pad(TileSelectionButton.pad);
            this.GetLabelCell().SetPadLeft(3);
            this.GetImage().SetScale(2);
            this.Pack();
        }

        internal TileSelectionButton(TileType tileType, ImageTextButtonStyle style) : 
            this(Tile.BaseTileType(tileType), style)
        {
            this.TileType = tileType;
        }
    }
}