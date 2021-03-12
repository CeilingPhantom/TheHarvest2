using Microsoft.Xna.Framework;
using System.Linq;
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
    public class TentativeFarmGrid : Grid, IInputable
    {
        FarmGrid farm;
        BoundlessSparseMatrix<Dictionary<Structure.StructureBuffs, int>> tileBuffsCountGrid = new BoundlessSparseMatrix<Dictionary<Structure.StructureBuffs, int>>();
        PlayerState playerState = PlayerState.Instance;
        PlayerCamera playerCamera = PlayerCamera.Instance;
        TileHighlighter tileHighlighter;
        TileType? currSelectedTileType = null;

        BoundlessSparseMatrix<bool> invalidTilesGrid = new BoundlessSparseMatrix<bool>();
        public (int X, int Y)[] InvalidTiles => this.invalidTilesGrid.AllValuesWithPos().Select(val => (val.X, val.Y)).ToArray();
        
        BoundlessSparseMatrix<bool> allTileChangesGrid = new BoundlessSparseMatrix<bool>();
        public List<WeakTile> AppliedTileChanges { get; private set; } = new List<WeakTile>();
        
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

            public bool IsEmpty()
            {
                return this.Positions.Count == 0;
            }
        }
        // dummy node as the first node in the linked list
        MouseDownChanges dummyChange = new MouseDownChanges();
        LinkedListNode<MouseDownChanges> currChangesNode;
        MouseDownChanges currChanges;
        LinkedList<MouseDownChanges> changeLog = new LinkedList<MouseDownChanges>();

        static readonly int inputPriority = 0;

        public TentativeFarmGrid(FarmGrid farm) : base()
        {
            InputManager.Instance.Register(this, TentativeFarmGrid.inputPriority);

            this.farm = farm;

            this.SubscribeTo<TileSelectionEvent>();
            this.SubscribeTo<TentativeFarmGridApplyChangesRequestEvent>();
            this.SubscribeTo<TentativeFarmGridOnEvent>();
            this.SubscribeTo<TentativeFarmGridOffEvent>();
            this.SubscribeTo<TentativeFarmGridUndoEvent>();
            this.SubscribeTo<TentativeFarmGridRedoEvent>();
        }

        public override void OnAddedToEntity()
        {
            this.tileHighlighter = this.AddComponent(new TileHighlighter(this));
            this.Enabled = false;
        }

        public override void OnEnabled()
        {
            // init grid for new tentative changes
            this.TileGrid = new BoundlessSparseMatrix<TileEntity>();
            this.tileBuffsCountGrid = new BoundlessSparseMatrix<Dictionary<Structure.StructureBuffs, int>>();
            this.allTileChangesGrid = new BoundlessSparseMatrix<bool>();
            foreach (var tile in this.farm.AllTiles())
            {
                // add "weak" clone/representation of existing tile
                var tileType = Tile.GetFutureTileType(tile);
                this.AddTile(new WeakTile(tileType, tile.X, tile.Y, this), true);
            }
            this.changeLog.Clear();
            this.currChangesNode = this.changeLog.AddFirst(dummyChange);

            this.tileHighlighter.Enabled = true;
        }

        public override void OnDisabled()
        {
            this.tileHighlighter.Enabled = false;

            if (this.TileGrid != null)
            {
                this.UpdateChangeList();
                // destroy all weak tiles
                foreach (var tileEntity in this.TileGrid.AllValues())
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
        }

        void ProcessInput()
        {
            // TODO rectangle selecting multiple tiles

            // need to check if mouse click collides with any UI
            if (InputManager.Instance.CanAcceptInput(TentativeFarmGrid.inputPriority))
            {
                this.tileHighlighter.HighlightTileAtMouse = true;
                if (Input.LeftMouseButtonDown)
                {
                    var pos = this.playerCamera.MouseToTilePosition();
                    if (!this.currChanges.Positions.ContainsKey(pos))
                    {
                        var x = (int) pos.X;
                        var y = (int) pos.Y;
                        var originalTileType = this.TileGrid[x, y] != null ? this.TileGrid[x, y].Tile.TileType : FarmDefaultTiler.DefaultTileType;
                        if (this.currSelectedTileType.Value == TileType.Destruct)
                        {
                            if (this.RemoveTile(x, y))
                            {
                                this.AddChange(x, y, originalTileType);
                            }
                        }
                        else if (this.currSelectedTileType.Value == TileType.Upgrade)
                        {
                            if (this.UpgradeTile(x, y))
                            {
                                this.AddChange(x, y, originalTileType);
                            }
                        }
                        else if (this.currSelectedTileType.Value == TileType.Reset)
                        {
                            if (this.ResetTile(x, y))
                            {
                                this.AddChange(x, y, originalTileType);
                            }
                        }
                        // else selected an actual (non utility) tile
                        // add new tile only if no existing tile or new tile is of different type
                        // and tile is placeable
                        // TODO better way to determine if existing tentative tile should be directly replaceable
                        else
                        {
                            var tmpTile = Tile.CreateTile(this.currSelectedTileType.Value, x, y, this);
                            if ((this.farm.GetTile(x, y) == null || 
                                !Tile.AreSameBaseTileType(Tile.GetFutureTileType(this.farm.GetTile(x, y)), this.currSelectedTileType.Value))
                                && 
                                (this.TileGrid[x, y] == null || 
                                !Tile.AreSameBaseTileType(this.TileGrid[x, y].Tile.TileType, this.currSelectedTileType.Value)) 
                                && 
                                tmpTile.IsPlaceable() 
                                && 
                                this.playerState.Money >= tmpTile.Cost)
                            {
                                this.AddTile(new WeakTile(this.currSelectedTileType.Value, x, y, this));
                                this.AddChange(x, y, originalTileType);
                            }
                        }
                    }
                }
            }
            else
            {
                this.tileHighlighter.HighlightTileAtMouse = false;
            }

            // add change to log when left click released, wherever it occurs
            if (Input.LeftMouseButtonReleased && !this.currChanges.IsEmpty())
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
                this.tileHighlighter.HighlightTileAtMouse = false;
            }
        }

        #region Grid Manipulation & Related Helpers

        public override TileEntity AddTile(Tile tile, bool isInit=false)
        {
            var tileType = tile.TileType;
            var x = tile.X;
            var y = tile.Y;
            if (isInit)
            {
                this.TileGrid[x, y] = new TileEntity(tile);
                this.Entity.Scene.AddEntity(this.TileGrid[x, y]);
            }
            else
            {
                this.RemoveTile(x, y);
                if (tileType != FarmDefaultTiler.DefaultTileType)
                {
                    this.TileGrid[x, y] = new TileEntity(tile);
                    this.Entity.Scene.AddEntity(this.TileGrid[x, y]);
                    EventManager.Instance.Publish(new AddMoneyEvent(-Tile.GetCost(tileType)));
                }
            }
            if (Tile.GetTileTypeGroup(tileType) == TileTypeGroup.Structure)
            {
                var tmpStructure = (Structure) Tile.CreateTile(tileType, x, y, this);
                this.AddTileBuffs(tmpStructure);
                // check if any existing invalid tiles are now valid
                this.CheckExistingInvalidTiles();
            }
            return this.TileGrid[x, y];
        }

        void AddTileBuffs(Structure structure)
        {
            foreach (var pos in structure.GetBuffedTilePositions())
            {
                if (this.tileBuffsCountGrid[pos.X, pos.Y] == null)
                {
                    this.tileBuffsCountGrid[pos.X, pos.Y] = new Dictionary<Structure.StructureBuffs, int>();
                }
                foreach (var buff in structure.BuffsList)
                {
                    this.tileBuffsCountGrid[pos.X, pos.Y].TryGetValue(buff, out int val);
                    this.tileBuffsCountGrid[pos.X, pos.Y][buff] = val + 1;
                }
            }
        }

        void CheckExistingInvalidTiles()
        {
            foreach (var val in this.InvalidTiles)
            {
                var x = val.X;
                var y = val.Y;
                var tmpTile = Tile.CreateTile(this.TileGrid[x, y].Tile.TileType, x, y, this);
                if (tmpTile.IsPlaceable())
                {
                    this.invalidTilesGrid.Remove(x, y);
                }
            }
        }

        public override bool RemoveTile(int x, int y)
        {
            if (this.TileGrid[x, y] != null)
            {
                var tileType = this.TileGrid[x, y].Tile.TileType;
                EventManager.Instance.Publish(new AddMoneyEvent(Tile.GetCost(tileType)));
                this.TileGrid[x, y].Destroy();
                this.TileGrid.Remove(x, y);
                this.invalidTilesGrid.Remove(x, y);
                if (Tile.GetTileTypeGroup(tileType) == TileTypeGroup.Structure)
                {
                    var tmpStructure = (Structure) Tile.CreateTile(tileType, x, y, this);
                    this.RemoveTileBuffs(tmpStructure);
                    // check for invalid tiles
                    this.CheckNewInvalidTiles(tmpStructure);
                }
                return true;
            }
            return false;
        }

        void RemoveTileBuffs(Structure structure)
        {
            foreach (var pos in structure.GetBuffedTilePositions())
            {
                foreach (var buff in structure.BuffsList)
                {
                    this.tileBuffsCountGrid[pos.X, pos.Y][buff] -= 1;
                }
            }
        }

        void CheckNewInvalidTiles(Structure structure)
        {
            foreach (var pos in structure.GetBuffedTilePositions())
            {
                var x = pos.X;
                var y = pos.Y;
                if (this.TileGrid[x, y] != null)
                {
                    var tmpTile = Tile.CreateTile(this.TileGrid[x, y].Tile.TileType, x, y, this);
                    if (!tmpTile.IsPlaceable())
                    {
                        this.invalidTilesGrid[x, y] = true;
                    }
                }
            }
        }

        bool UpgradeTile(int x, int y)
        {
            TileType upgradedTileType;
            int upgradeCost;
            if (this.TileGrid[x, y] != null && 
                Tile.GetUpgradedTileType(this.TileGrid[x, y].Tile.TileType, out upgradedTileType) && 
                Tile.GetUpgradeCost(this.TileGrid[x, y].Tile.TileType, upgradedTileType, out upgradeCost) && 
                this.playerState.Money >= upgradeCost)
            {
                this.AddTile(new WeakTile(upgradedTileType, x, y, this));
                return true;
            }
            return false;
        }

        bool ResetTile(int x, int y)
        {
            // reset only if current tile is not same as tentative tile
            if (!(this.farm.GetTile(x, y) == null && this.TileGrid[x, y] == null || 
                this.farm.GetTile(x, y) != null && this.TileGrid[x, y] != null && this.farm.GetTile(x, y).TileType == this.TileGrid[x, y].Tile.TileType))
            {
                this.RemoveTile(x, y);
                if (this.farm.GetTile(x, y) != null)
                {
                    var tileType = Tile.GetFutureTileType(this.farm.GetTile(x, y));
                    this.AddTile(new WeakTile(tileType, x, y, this));
                }
                return true;
            }
            return false;
        }

        public override Structure.StructureBuffs GetTileBuffs(int x, int y)
        {
            Structure.StructureBuffs buffs = 0;
            if (this.tileBuffsCountGrid[x, y] != null)
            {
                foreach (var entry in this.tileBuffsCountGrid[x, y])
                {
                    var buff = entry.Key;
                    var count = entry.Value;
                    if (count > 0)
                    {
                        buffs |= buff;
                    }
                }
            }
            return buffs;
        }

        bool ValidState()
        {
            return this.invalidTilesGrid.IsEmpty();
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
            if (this.currChanges.TileType != originalTileType)
            {
                this.currChanges.AddPosition(x, y, originalTileType);
                // no change if the objects in both the tentative and actual grids are the same
                if ((this.TileGrid[x, y] == null && this.farm.GetTile(x, y) == null) 
                    || 
                    (this.TileGrid[x, y] != null && this.farm.GetTile(x, y) != null && this.TileGrid[x, y].Tile.TileType == Tile.GetFutureTileType(this.farm.GetTile(x, y))))
                {
                    this.allTileChangesGrid[x, y] = false;
                }
                else
                {
                    this.allTileChangesGrid[x, y] = true;
                }
            }
        }

        void UpdateChangeList()
        {
            this.AppliedTileChanges = new List<WeakTile>();
            foreach (var val in this.allTileChangesGrid.AllValuesWithPos())
            {
                var change = val.Val;
                var x = val.X;
                var y = val.Y;
                if (change)
                {
                    if (this.TileGrid[x, y] != null)
                    {
                        this.AppliedTileChanges.Add(new WeakTile(this.TileGrid[x, y].Tile));
                    }
                    else
                    {
                        this.AppliedTileChanges.Add(new WeakTile(TileType.Destruct, x, y, null));
                    }
                }
            }

            /* what was this for?
            foreach (var tileEntity in this.grid.AllValues())
            {
                var tile = (WeakTile) tileEntity.Tile;
                if (this.allTileChanges[tile.X, tile.Y])
                {
                    this.AppliedTileChanges.Add(tile);
                }
            }
            */

            this.TileBuffsGrid = new BoundlessSparseMatrix<Structure.StructureBuffs>();
            foreach (var val in this.tileBuffsCountGrid.AllValuesWithPos())
            {
                var tileBuffDict = val.Val;
                var x = val.X;
                var y = val.Y;
                TileBuffsGrid[x, y] = this.GetTileBuffs(x, y);
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
                    this.AddTile(new WeakTile(originalTileType, x, y, this));
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
                    if (tileType == TileType.Destruct)
                    {
                        this.RemoveTile(x, y);
                    }
                    else if (tileType == TileType.Upgrade)
                    {
                        TileType upgradedTileType;
                        int upgradeCost;
                        if (this.TileGrid[x, y] != null && 
                            Tile.GetUpgradedTileType(this.TileGrid[x, y].Tile.TileType, out upgradedTileType) && 
                            Tile.GetUpgradeCost(this.TileGrid[x, y].Tile.TileType, upgradedTileType, out upgradeCost) && 
                            this.playerState.Money >= upgradeCost)
                        {
                            this.AddTile(new WeakTile(upgradedTileType, x, y, this));
                        }
                    }
                    else if (tileType == TileType.Reset)
                    {
                        this.ResetTile(x, y);
                    }
                    else
                    {
                        this.AddTile(new WeakTile(tileType, x, y, this));
                    }
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

        public override void ProcessEvent(TentativeFarmGridApplyChangesRequestEvent e)
        {
            EventManager.Instance.Publish(new TentativeFarmGridApplyChangesResponseEvent(this.ValidState()));
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