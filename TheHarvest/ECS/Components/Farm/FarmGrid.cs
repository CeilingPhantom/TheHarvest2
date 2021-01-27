using Microsoft.Xna.Framework.Input;
using System;
using Nez;

using TheHarvest.ECS.Components.Player;
using TheHarvest.ECS.Components.Tiles;
using TheHarvest.ECS.Entities;
using TheHarvest.Events;
using TheHarvest.Util;

namespace TheHarvest.ECS.Components.Farm
{
    public class FarmGrid : EventSubscribingComponent
    {
        public BoundlessSparseMatrix<TileEntity> Grid { get; } = new BoundlessSparseMatrix<TileEntity>();
        PlayerState playerState = PlayerState.Instance;
        PlayerCamera playerCamera = PlayerCamera.Instance;

        FastList<TileEntity> initTileEntities = new FastList<TileEntity>();

        TileHighlighter tileHighlighter;
        TileType? currSelectedTileType = null;
        BoundlessSparseMatrix<TileEntity> TentativeGrid;

        public FarmGrid() : base()
        {
            EventManager.Instance.SubscribeTo<TileSelectionEvent>(this);
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            this.tileHighlighter = this.GetComponent<TileHighlighter>();
            
            this.AddTile(Tile.CreateTile(TileType.Grass, 0, 0));
            
            this.AttachInitTileEntitiesToScene();
        }

        private void AttachInitTileEntitiesToScene()
        {
            for (var i = 0; i < this.initTileEntities.Length; ++i)
                this.Entity.Scene.AddEntity(this.initTileEntities[i]);
            this.initTileEntities.Clear();
        }

        public TileEntity AddTile(Tile tile)
        {
            tile.FarmGrid = this;
            var tileEntity = new TileEntity(tile);
            this.Grid[tile.X, tile.Y] = tileEntity;
            if (this.Entity != null && this.Entity.Scene != null)
                this.Entity.Scene.AddEntity(tileEntity);
            else
                this.initTileEntities.Add(tileEntity);
            return this.Grid[tile.X, tile.Y];
        }

        public void RemoveTile(Tile tile)
        {
            this.Grid.Remove(tile.X, tile.Y);
        }

        public void RemoveTile(int x, int y)
        {
            if (this.Grid[x, y] == null)
                return;
            Tile tile = this.Grid[x, y].Tile;
            this.RemoveTile(tile);
        }

        public TileEntity ReplaceTile(Tile tile, TileType newTileType)
        {
            int x = tile.X;
            int y = tile.Y;
            return ReplaceTile(x, y, newTileType);
        }

        public TileEntity ReplaceTile(int x, int y, TileType newTileType)
        {
            if (this.Grid[x, y] != null) {
                Tile tile = this.Grid[x, y].Tile;
                if (tile.TileType == newTileType)
                    return null;
                this.RemoveTile(tile);
                tile.Entity.Destroy();
            }
            if (newTileType != FarmDefaultTiler.DefaultTileType)
                return this.AddTile(Tile.CreateTile(newTileType, x, y));
            return null;
        }

        /*
        void EnableTentativeGrid(bool enable)
        {}
        */

        public override void Update()
        {
            // handle events
            base.Update();
            if (this.currSelectedTileType.HasValue)
            {
                if (!this.tileHighlighter.Enabled)
                    this.tileHighlighter.Enabled = true;
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    System.Diagnostics.Debug.WriteLine(this.currSelectedTileType);
                    var p = this.playerCamera.MouseToTilePosition();
                    System.Diagnostics.Debug.WriteLine(p);
                    this.ReplaceTile((int) p.X, (int) p.Y, this.currSelectedTileType.Value);
                }
                else if (Mouse.GetState().RightButton == ButtonState.Pressed)
                {
                    this.currSelectedTileType = null;
                    this.tileHighlighter.Enabled = false;
                    System.Diagnostics.Debug.WriteLine("curr selected tile cleared");
                }
            }
        }

        #region Event Processing

        public override void ProcessEvent(TileSelectionEvent e)
        {
            //System.Diagnostics.Debug.WriteLine(e.TileType);
            this.currSelectedTileType = e.TileType;
        }

        #endregion
    }
}