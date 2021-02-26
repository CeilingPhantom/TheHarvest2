using Microsoft.Xna.Framework;
using System;
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
        
        BoundlessSparseMatrix<bool> allChanges;
        public List<WeakTile> AppliedChanges { get; private set; } = new List<WeakTile>();
        
        // to ensure that tiles are only manipulated once per mousedown
        struct MouseDownChanges
        {
            public TileType TileType { get; }
            public Dictionary<Vector2, TileType> Positions { get; }

            public MouseDownChanges(TileType tileType)
            {
                this.TileType = tileType;
                this.Positions = new Dictionary<Vector2, TileType>();
            }

            public void AddPosition(int x, int y, TileType originalTileType)
            {
                var pos = new Vector2(x, y);
                this.Positions[pos] = originalTileType;
            }

            public void AddPosition(Vector2 pos, TileType originalTileType)
            {
                this.Positions[pos] = originalTileType;
            }
        }
        // dummy node as the first node in the linked list
        MouseDownChanges dummyChange = new MouseDownChanges();
        LinkedListNode<MouseDownChanges> currChangesNode;
        MouseDownChanges currChanges;
        LinkedList<MouseDownChanges> changeLog = new LinkedList<MouseDownChanges>();

        static readonly int inputPriority = 0;

        public TentativeFarmGrid(FarmGrid farm)
        {
            InputManager.Instance.Register(this, TentativeFarmGrid.inputPriority);

            this.Enabled = false;

            this.farm = farm;
            EventManager.Instance.SubscribeTo<TileSelectionEvent>(this);
            EventManager.Instance.SubscribeTo<TentativeFarmGridOnEvent>(this);
            EventManager.Instance.SubscribeTo<TentativeFarmGridOffEvent>(this);
            EventManager.Instance.SubscribeTo<TentativeFarmGridUndoEvent>(this);
            EventManager.Instance.SubscribeTo<TentativeFarmGridRedoEvent>(this);
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
            this.allChanges = new BoundlessSparseMatrix<bool>();
            foreach (var tileEntity in this.farm.Grid.AllValues())
            {
                var tile = tileEntity.Tile;
                // add "weak" clone/representation of existing tile
                var tileType = Tile.GetFutureTileType(tile);
                this.AddTile(tileType, tile.X, tile.Y, true);
            }
            this.changeLog.Clear();
            this.currChangesNode = this.changeLog.AddFirst(dummyChange);
        }

        public override void OnDisabled()
        {
            if (this.grid != null)
            {
                this.UpdateChangeList();
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
            // TODO rectangle selecting multiple tiles

            // need to check if mouse click collides with any UI
            if (InputManager.Instance.CanAcceptInput(TentativeFarmGrid.inputPriority))
            {
                this.tileHighlighter.Enabled = true;
                if (Input.LeftMouseButtonDown)
                {
                    var pos = this.playerCamera.MouseToTilePosition();
                    if (!this.currChanges.Positions.ContainsKey(pos))
                    {
                        var x = (int) pos.X;
                        var y = (int) pos.Y;
                        var originalTileType = this.grid[x, y] != null ? this.grid[x, y].Tile.TileType : FarmDefaultTiler.DefaultTileType;
                        if (this.currSelectedTileType.Value == TileType.Destruct)
                        {
                            this.RemoveTile(x, y);
                            this.AddChange(x, y, originalTileType);
                        }
                        else if (this.currSelectedTileType.Value == TileType.Upgrade)
                        {
                            TileType upgradedTileType;
                            int upgradeCost;
                            if (this.grid[x, y] != null && 
                                Tile.GetUpgradedTileType(this.grid[x, y].Tile.TileType, out upgradedTileType) && 
                                Tile.GetUpgradeCost(this.grid[x, y].Tile.TileType, upgradedTileType, out upgradeCost) && 
                                this.playerState.Money >= upgradeCost)
                            {
                                this.AddTile(upgradedTileType, x, y);
                                this.AddChange(x, y, originalTileType);
                            }
                        }
                        else if (this.currSelectedTileType.Value == TileType.Reset)
                        {
                            this.ResetTile(x, y);
                            this.AddChange(x, y, originalTileType);
                        }
                        // add new tile only if no existing tile or new tile is of different type
                        else if ((this.farm.Grid[x, y] == null || 
                            !Tile.AreSameBaseTileType(Tile.GetFutureTileType(this.farm.Grid[x, y].Tile), this.currSelectedTileType.Value))
                            && 
                            (this.grid[x, y] == null || 
                            !Tile.AreSameBaseTileType(this.grid[x, y].Tile.TileType, this.currSelectedTileType.Value)) 
                            && 
                            this.playerState.Money >= Tile.GetCost(this.currSelectedTileType.Value))
                        {
                            this.AddTile(this.currSelectedTileType.Value, x, y);
                            this.AddChange(x, y, originalTileType);
                        }
                    }
                }
            }
            else
            {
                this.tileHighlighter.Enabled = false;
            }

            // add change to log when left click released, wherever it occurs
            if (Input.LeftMouseButtonReleased)
            {
                // remove all nodes after this
                while (this.currChangesNode.Next != null)
                {
                    this.changeLog.Remove(this.currChangesNode.Next);
                }
                this.currChangesNode = this.changeLog.AddLast(this.currChanges);
                this.currChanges = new MouseDownChanges(this.currSelectedTileType.Value);
            }

            // clear selected tile regardless of where right click occurs
            if (Input.RightMouseButtonPressed)
            {
                this.currSelectedTileType = null;
                this.tileHighlighter.Enabled = false;
            }
        }

        #region Grid Manipulation & Related Helpers

        TileEntity AddTile(TileType tileType, int x, int y, bool isInit=false)
        {
            if (isInit)
            {
                this.grid[x, y] = new TileEntity(new WeakTile(tileType, x, y));
                this.Entity.Scene.AddEntity(this.grid[x, y]);
            }
            else
            {
                this.RemoveTile(x, y);
                if (tileType != FarmDefaultTiler.DefaultTileType)
                {
                    this.grid[x, y] = new TileEntity(new WeakTile(tileType, x, y));
                    this.Entity.Scene.AddEntity(this.grid[x, y]);
                    EventManager.Instance.Publish(new AddMoneyEvent(-Tile.GetCost(tileType)));
                }
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
            }
        }

        void ResetTile(int x, int y)
        {
            this.RemoveTile(x, y);
            if (this.farm.Grid[x, y] != null)
            {
                var tileType = Tile.GetFutureTileType(this.farm.GetTile(x, y));
                this.AddTile(tileType, x, y);
            }
        }

        #endregion

        #region Changelogs and Related

        bool IsDummyChange(MouseDownChanges changes)
        {
            return changes.Positions == null;
        }

        // call this when adding a new change, 
        // i.e. want to set a changes grid at x, y to be true
        void AddChange(int x, int y, TileType originalTileType)
        {
            this.currChanges.AddPosition(x, y, originalTileType);
            // no change if the objects in both the tentative and actual grids are the same
            if ((this.grid[x, y] == null && this.farm.Grid[x, y] == null) 
                || 
                (this.grid[x, y] != null && this.farm.Grid[x, y] != null && this.grid[x, y].Tile.TileType == Tile.GetFutureTileType(this.farm.Grid[x, y].Tile)))
            {
                this.allChanges[x, y] = false;
            }
            else
            {
                this.allChanges[x, y] = true;
            }
        }

        void UpdateChangeList()
        {
            this.AppliedChanges.Clear();
            foreach (var change in this.allChanges.AllValuesWithPos())
            {
                var val = change.Val;
                var x = change.X;
                var y = change.Y;
                if (val)
                {
                    if (this.grid[x, y] != null)
                    {
                        this.AppliedChanges.Add((WeakTile) this.grid[x, y].Tile);
                    }
                    else
                    {
                        this.AppliedChanges.Add(new WeakTile(TileType.Destruct, x, y));
                    }
                }
            }
            foreach (var tileEntity in this.grid.AllValues())
            {
                var tile = (WeakTile) tileEntity.Tile;
                if (this.allChanges[tile.X, tile.Y])
                {
                    this.AppliedChanges.Add(tile);
                }
            }
        }

        void Undo()
        {
            if (!IsDummyChange(this.currChangesNode.Value))
            {
                foreach (var entry in this.currChangesNode.Value.Positions)
                {
                    var pos = entry.Key;
                    var x = (int) pos.X;
                    var y = (int) pos.Y;
                    var originalTileType = entry.Value;
                    this.AddTile(originalTileType, x, y);
                }
                this.currChangesNode = this.currChangesNode.Previous;
            }
        }

        void Redo()
        {
            if (this.currChangesNode.Next != null)
            {
                this.currChangesNode = this.currChangesNode.Next;
                var tileType = this.currChangesNode.Value.TileType;
                foreach (var pos in this.currChangesNode.Value.Positions.Keys)
                {
                    var x = (int) pos.X;
                    var y = (int) pos.Y;
                    this.AddTile(tileType, x, y);
                }
            }
        }

        #endregion

        #region Event Processing

        public override void ProcessEvent(TileSelectionEvent e)
        {
            this.currSelectedTileType = e.TileType;
            this.currChanges = new MouseDownChanges(this.currSelectedTileType.Value);
        }

        public override void ProcessEvent(TentativeFarmGridOffEvent e)
        {
            // this order of enabling is important
            this.farm.Enabled = true;
            this.Enabled = false;
            this.currSelectedTileType = null;
        }

        public override void ProcessEvent(TentativeFarmGridUndoEvent e)
        {
            this.Undo();
        }

        public override void ProcessEvent(TentativeFarmGridRedoEvent e)
        {
            this.Redo();
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