using System.Collections.Generic;
using Nez;

using TheHarvest.ECS.Entities;
using TheHarvest.ECS.Components.Player;
using TheHarvest.ECS.Components.Tiles;
using TheHarvest.Events;
using TheHarvest.Util;
using TheHarvest.Util.Input;

namespace TheHarvest.ECS.Components.Farm
{
    public class TentativeFarmGrid : EventSubscribingComponent, IInputable
    {
        FarmGrid farm;
        BoundlessSparseMatrix<TileEntity> grid;
        PlayerState playerState = PlayerState.Instance;
        PlayerCamera playerCamera = PlayerCamera.Instance;
        TileHighlighter tileHighlighter;
        TileType? currSelectedTileType = null;
        BoundlessSparseMatrix<bool> changes;
        public List<WeakTile> ChangesList { get; private set; } = new List<WeakTile>();

        static readonly int inputPriority = 0;

        public TentativeFarmGrid(FarmGrid farm)
        {
            InputManager.Instance.Register(this, TentativeFarmGrid.inputPriority);

            this.Enabled = false;

            this.farm = farm;
            EventManager.Instance.SubscribeTo<TileSelectionEvent>(this);
            EventManager.Instance.SubscribeTo<EditFarmOnEvent>(this);
            EventManager.Instance.SubscribeTo<EditFarmOffEvent>(this);
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            this.tileHighlighter = this.GetComponent<TileHighlighter>();
        }

        public override void OnEnabled()
        {
            // init grid for new tentative changes
            this.grid = new BoundlessSparseMatrix<TileEntity>();
            this.changes = new BoundlessSparseMatrix<bool>();
            foreach (var tileEntity in this.farm.Grid.AllValues())
            {
                var tile = tileEntity.Tile;
                // add "weak" clone/representation of existing tile
                var tileType = this.GetFutureTileType(tile);
                this.AddTile(tileType, tile.X, tile.Y, true);
            }
        }

        public override void OnDisabled()
        {
            if (this.grid != null)
            {
                this.GetChanges();
                // destroy all weak tiles
                foreach (var tileEntity in this.grid.AllValues())
                {
                    tileEntity.Destroy();
                }
            }
        }

        public override void Update()
        {
            // handle events
            base.Update();

            if (this.currSelectedTileType.HasValue)
            {
                this.ProcessInput();
            }
            else
            {
                this.tileHighlighter.Enabled = false;
            }
        }

        void ProcessInput()
        {
            // need to check if mouse click collides with any UI
            if (InputManager.Instance.CanAcceptInput(TentativeFarmGrid.inputPriority))
            {
                this.tileHighlighter.Enabled = true;
                if (Input.LeftMouseButtonDown)
                {
                    var p = this.playerCamera.MouseToTilePosition();
                    this.AddTile(this.currSelectedTileType.Value, (int) p.X, (int) p.Y);
                }
            }
            else
            {
                this.tileHighlighter.Enabled = false;
            }

            // clear selected tile regardless of where click occurs
            if (Input.RightMouseButtonPressed)
            {
                this.currSelectedTileType = null;
                this.tileHighlighter.Enabled = false;
            }
        }

        #region Grid Manipulation

        TileEntity AddTile(TileType tileType, int x, int y, bool isInit=false)
        {
            if (isInit || this.playerState.Money >= Tile.GetCost(tileType))
            {
                this.RemoveTile(x, y);
                this.grid[x, y] = new TileEntity(new WeakTile(tileType, x, y));
                this.Entity.Scene.AddEntity(this.grid[x, y]);
                // if it is a change in tiletype
                if (!isInit)
                {
                    EventManager.Instance.Publish(new AddMoneyEvent(-Tile.GetCost(tileType)));
                    this.changes[x, y] = this.farm.Grid[x, y] == null || this.GetFutureTileType(this.farm.Grid[x, y].Tile) != tileType;
                }
            }
            return this.grid[x, y];
        }

        void RemoveTile(int x, int y)
        {
            if (this.grid[x, y] != null)
            {
                EventManager.Instance.Publish(new AddMoneyEvent(-Tile.GetCost(this.grid[x, y].Tile.TileType)));
                this.grid[x, y].Destroy();
                this.grid.Remove(x, y);
            }
        }

        void GetChanges()
        {
            this.ChangesList.Clear();
            foreach (var tileEntity in this.grid.AllValues())
            {
                var tile = (WeakTile) tileEntity.Tile;
                if (this.changes[tile.X, tile.Y])
                {
                    this.ChangesList.Add(tile);
                }
            }
        }

        #endregion

        TileType GetFutureTileType(Tile tile)
        {
            // use the tile that it will advance into, if it exists
            return tile.IsAdvancing ? tile.AdvancingType : tile.TileType;
        }

        #region Event Processing

        public override void ProcessEvent(TileSelectionEvent e)
        {
            this.currSelectedTileType = e.TileType;
        }

        public override void ProcessEvent(EditFarmOnEvent e)
        {}

        public override void ProcessEvent(EditFarmOffEvent e)
        {
            this.Enabled = false;
            this.farm.Enabled = true;
            this.currSelectedTileType = null;
        }

        #endregion

        #region IInputable

        public bool InputCollision()
        {
            // grid takes up whole window anyways
            return true;
        }

        #endregion
    }
}