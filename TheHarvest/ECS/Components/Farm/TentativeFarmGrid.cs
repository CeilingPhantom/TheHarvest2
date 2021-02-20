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

        // so tiles only get upgraded once per mousedown
        HashSet<(int x, int y)> currMouseDownUpgraded = new HashSet<(int x, int y)>();

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
                    var x = (int) p.X;
                    var y = (int) p.Y;
                    if (this.currSelectedTileType.Value == TileType.Destruct)
                    {
                        this.RemoveTile(x, y);
                    }
                    else if (this.currSelectedTileType.Value == TileType.Upgrade)
                    {
                        TileType upgradedTileType;
                        int upgradeCost;
                        // upgradeable if grid's existing (future) tile can be upgraded to it
                        // or current tentative tile can be upgraded to it
                        if (!this.currMouseDownUpgraded.Contains((x, y)) && 
                            (
                                (this.farm.Grid[x, y] != null && 
                                Tile.GetUpgradedTileType(this.GetFutureTileType(this.farm.Grid[x, y].Tile), out upgradedTileType) && 
                                Tile.GetUpgradeCost(this.GetFutureTileType(this.farm.Grid[x, y].Tile), upgradedTileType, out upgradeCost))
                                ||
                                (this.grid[x, y] != null && 
                                Tile.GetUpgradedTileType(this.grid[x, y].Tile.TileType, out upgradedTileType) && 
                                Tile.GetUpgradeCost(this.grid[x, y].Tile.TileType, upgradedTileType, out upgradeCost))
                            ) 
                            && 
                            this.playerState.Money >= upgradeCost)
                        {
                            this.currMouseDownUpgraded.Add((x, y));
                            this.AddTile(upgradedTileType, x, y);
                        }
                    }
                    // add new tile only if no existing tile or new tile is of different type
                    else if ((this.farm.Grid[x, y] == null || 
                        !Tile.AreSameBaseTileType(this.GetFutureTileType(this.farm.Grid[x, y].Tile), this.currSelectedTileType.Value))
                        && 
                        (this.grid[x, y] == null || 
                        !Tile.AreSameBaseTileType(this.grid[x, y].Tile.TileType, this.currSelectedTileType.Value)) 
                        && 
                        this.playerState.Money >= Tile.GetCost(this.currSelectedTileType.Value))
                    {
                        this.AddTile(this.currSelectedTileType.Value, x, y);
                    }
                }
                else
                {
                    if (this.currSelectedTileType.Value == TileType.Upgrade)
                    {
                        this.currMouseDownUpgraded.Clear();
                    }
                }
            }
            else
            {
                this.tileHighlighter.Enabled = false;
            }

            // clear selected tile regardless of where click occurs
            if (Input.RightMouseButtonPressed)
            {
                if (this.currSelectedTileType.Value == TileType.Upgrade)
                {
                    this.currMouseDownUpgraded.Clear();
                }
                this.currSelectedTileType = null;
                this.tileHighlighter.Enabled = false;
            }
        }

        #region Grid Manipulation

        TileEntity AddTile(TileType tileType, int x, int y, bool isInit=false)
        {
            if (isInit)
            {
                this.grid[x, y] = new TileEntity(new WeakTile(tileType, x, y));
                this.Entity.Scene.AddEntity(this.grid[x, y]);
            }
            else if (this.playerState.Money >= Tile.GetCost(tileType))
            {
                this.RemoveTile(x, y);
                this.grid[x, y] = new TileEntity(new WeakTile(tileType, x, y));
                this.Entity.Scene.AddEntity(this.grid[x, y]);
                EventManager.Instance.Publish(new AddMoneyEvent(-Tile.GetCost(tileType)));
                this.changes[x, y] = true;
            }
            return this.grid[x, y];
        }

        void RemoveTile(int x, int y)
        {
            if (this.grid[x, y] != null)
            {
                EventManager.Instance.Publish(new AddMoneyEvent(Tile.GetCost(this.grid[x, y].Tile.TileType)));
                this.grid[x, y].Destroy();
                this.grid.Remove(x, y);
                this.changes[x, y] = true;
            }
        }

        void GetChanges()
        {
            this.ChangesList.Clear();
            foreach (var change in this.changes.AllValuesWithPos())
            {
                var val = change.Val;
                var x = change.X;
                var y = change.Y;
                if (val)
                {
                    if (this.grid[x, y] != null)
                    {
                        this.ChangesList.Add((WeakTile) this.grid[x, y].Tile);
                    }
                    else
                    {
                        this.ChangesList.Add(new WeakTile(TileType.Destruct, x, y));
                    }
                }
            }
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
            // this order of enabling is important
            this.farm.Enabled = true;
            this.Enabled = false;
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