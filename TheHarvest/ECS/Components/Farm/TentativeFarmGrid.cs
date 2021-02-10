using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

using TheHarvest.ECS.Entities;
using TheHarvest.ECS.Components.Player;
using TheHarvest.ECS.Components.Tiles;
using TheHarvest.Events;
using TheHarvest.Util;

namespace TheHarvest.ECS.Components.Farm
{
    public class TentativeFarmGrid : EventSubscribingComponent
    {
        FarmGrid farm;
        BoundlessSparseMatrix<TileEntity> grid;
        PlayerState playerState = PlayerState.Instance;
        PlayerCamera playerCamera = PlayerCamera.Instance;
        TileHighlighter tileHighlighter;
        TileType? currSelectedTileType = null;
        int totalCost = 0;
        BoundlessSparseMatrix<bool> changes;
        public List<WeakTile> ChangesList { get; private set; } = new List<WeakTile>();

        public TentativeFarmGrid(FarmGrid farm)
        {
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
                if (!this.tileHighlighter.Enabled)
                {
                    this.tileHighlighter.Enabled = true;
                }
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    var p = this.playerCamera.MouseToTilePosition();
                    //System.Diagnostics.Debug.WriteLine(p);
                    //System.Diagnostics.Debug.WriteLine(this.currSelectedTileType);
                    this.AddTile(this.currSelectedTileType.Value, (int) p.X, (int) p.Y);
                }
                else if (Mouse.GetState().RightButton == ButtonState.Pressed)
                {
                    this.currSelectedTileType = null;
                    this.tileHighlighter.Enabled = false;
                }
            }
        }

        #region Grid Manipulation

        TileEntity AddTile(TileType tileType, int x, int y, bool isInit=false)
        {
            this.RemoveTile(x, y);
            if (isInit || this.totalCost + Tile.GetCost(tileType) <= this.playerState.Money) {
                this.grid[x, y] = new TileEntity(new WeakTile(tileType, x, y));
                this.Entity.Scene.AddEntity(this.grid[x, y]);
                // if it is a change in tiletype
                if (!isInit) {
                    this.totalCost += Tile.GetCost(tileType);
                    this.changes[x, y] = this.farm.Grid[x, y] == null || this.GetFutureTileType(this.farm.Grid[x, y].Tile) != tileType;
                }
            }
            return this.grid[x, y];
        }

        void RemoveTile(int x, int y)
        {
            if (this.grid[x, y] != null)
            {
                this.totalCost -= this.grid[x, y].Tile.Cost;
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
        }

        #endregion

        TileType GetFutureTileType(Tile tile)
        {
            // use the tile that it will advance into, if it exists
            return tile.IsAdvancing ? tile.AdvancingType : tile.TileType;
        }
    }
}