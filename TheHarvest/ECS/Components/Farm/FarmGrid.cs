using Microsoft.Xna.Framework;
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
        public BoundlessSparseMatrix<TileEntity> Grid { get; private set; } = new BoundlessSparseMatrix<TileEntity>();
        PlayerState playerState = PlayerState.Instance;
        PlayerCamera playerCamera = PlayerCamera.Instance;

        FastList<TileEntity> initTileEntities = new FastList<TileEntity>();

        TentativeFarmGrid tentativeFarmGrid;

        public FarmGrid() : base()
        {
            EventManager.Instance.SubscribeTo<EditFarmOnEvent>(this);
            EventManager.Instance.SubscribeTo<EditFarmOffEvent>(this);
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            this.tentativeFarmGrid = this.GetComponent<TentativeFarmGrid>();
            
            this.AddTile(Tile.CreateTile(TileType.Grass, 0, 0));
            
            this.AttachInitTileEntitiesToScene();
        }

        private void AttachInitTileEntitiesToScene()
        {
            for (var i = 0; i < this.initTileEntities.Length; ++i)
            {
                this.Entity.Scene.AddEntity(this.initTileEntities[i]);
            }
            this.initTileEntities.Clear();
        }

        #region Grid Accessors and Manipulation

        public Tile GetTile(int x, int y)
        {
            if (this.Grid[x, y] == null)
            {
                return null;
            }
            return this.Grid[x, y].Tile;
        }

        public Tile GetTile(Vector2 pos)
        {
            return this.GetTile((int) pos.X, (int) pos.Y);
        }

        public TileEntity AddTile(Tile tile)
        {
            if (this.Grid[tile.X, tile.Y] != null) {
                return null;
            }
            tile.FarmGrid = this;
            var tileEntity = new TileEntity(tile);
            this.Grid[tile.X, tile.Y] = tileEntity;
            if (this.Entity != null && this.Entity.Scene != null)
            {
                this.Entity.Scene.AddEntity(tileEntity);
            }
            else
            {
                this.initTileEntities.Add(tileEntity);
            }
            return this.Grid[tile.X, tile.Y];
        }

        public void RemoveTile(Tile tile)
        {
            tile.Entity.Destroy();
            this.Grid.Remove(tile.X, tile.Y);
        }

        public void RemoveTile(int x, int y)
        {
            if (this.Grid[x, y] == null)
            {
                return;
            }
            Tile tile = this.Grid[x, y].Tile;
            this.RemoveTile(tile);
        }

        public TileEntity ReplaceTile(Tile newTile)
        {
            int x = newTile.X;
            int y = newTile.Y;
            if (this.Grid[x, y] != null)
            {
                Tile tile = this.Grid[x, y].Tile;
                this.RemoveTile(tile);
            }
            if (newTile.TileType != FarmDefaultTiler.DefaultTileType)
            {
                return this.AddTile(newTile);
            }
            return null;
        }

        void ApplyTentativeGridChanges()
        {
            foreach (var weakTile in this.tentativeFarmGrid.ChangesList)
            {
                // if tile was set to be removed
                if (weakTile.TileType == TileType.Destruct)
                {
                    this.RemoveTile(weakTile.X, weakTile.Y);
                }
                // else new tile set
                else
                {
                    // check if new tile requires intermediate advancing before turning into that tile type
                    var advancesFromTileType = Tile.AdvancesFrom(weakTile.TileType);
                    if (advancesFromTileType.HasValue)
                    {
                        this.ReplaceTile(Tile.CreateTile(advancesFromTileType.Value, weakTile.X, weakTile.Y, true, weakTile.TileType, 0));
                    }
                    else
                    {
                        this.ReplaceTile(Tile.CreateTile(weakTile.TileType, weakTile.X, weakTile.Y));
                    }
                }
            }
        }

        #endregion

        public override void OnEnabled()
        {
            // enable all tiles
            // only if not coming out of tentative farm grid
            // this fixes removed tiles still showing for 1 tick
            // after applying tentative changes
            if (!this.tentativeFarmGrid.Enabled)
            {
                foreach (var tileEntity in this.Grid.AllValues())
                {
                    tileEntity.Enabled = true;
                }
            }
        }

        public override void OnDisabled()
        {
            // disable all tiles
            foreach (var tileEntity in this.Grid.AllValues())
            {
                tileEntity.Enabled = false;
            }
        }

        public override void Update()
        {
            // handle events
            base.Update();
        }

        #region Event Processing

        public override void ProcessEvent(EditFarmOnEvent e)
        {
            this.Enabled = false;
            this.tentativeFarmGrid.Enabled = true;
        }

        public override void ProcessEvent(EditFarmOffEvent e)
        {
            // apply tentative changes
            if (e.ApplyChanges)
            {
                this.ApplyTentativeGridChanges();
            }
            // call this to truly reenable the grid
            OnEnabled();
        }

        #endregion
    }
}