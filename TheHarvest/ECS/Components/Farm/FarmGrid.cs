using Microsoft.Xna.Framework;
using System.Linq;
using Nez;

using TheHarvest.ECS.Components.Player;
using TheHarvest.ECS.Components.Tiles;
using TheHarvest.ECS.Entities;
using TheHarvest.Events;
using TheHarvest.Util;

namespace TheHarvest.ECS.Components.Farm
{
    public class FarmGrid : Grid
    {
        PlayerState playerState = PlayerState.Instance;

        FastList<TileEntity> initTileEntities = new FastList<TileEntity>();

        TentativeFarmGrid tentativeFarmGrid;

        public FarmGrid() : base()
        {
            EventManager.Instance.SubscribeTo<TentativeFarmGridOnEvent>(this);
            EventManager.Instance.SubscribeTo<TentativeFarmGridOffEvent>(this);
            
            EventManager.Instance.SubscribeTo<NewSeasonEvent>(this);
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

        public override TileEntity AddTile(Tile tile, bool isInit=false)
        {
            var tileType = tile.TileType;
            var x = tile.X;
            var y = tile.Y;
            this.RemoveTile(x, y);
            if (tileType != FarmDefaultTiler.DefaultTileType)
            {
                tile.Grid = this;
                var tileEntity = new TileEntity(tile);
                this.TileGrid[x, y] = tileEntity;
                if (this.Entity != null && this.Entity.Scene != null)
                {
                    this.Entity.Scene.AddEntity(tileEntity);
                }
                else
                {
                    this.initTileEntities.Add(tileEntity);
                    // also initialise tile buff matrix
                    if (Tile.GetTileTypeGroup(tileType) == TileTypeGroup.Structure)
                    {
                        this.TileBuffsGrid[x, y] |= ((Structure) tile).Buffs;
                    }
                }
            }
            return this.TileGrid[x, y];
        }

        public override bool RemoveTile(int x, int y)
        {
            if (this.TileGrid[x, y] == null)
            {
                return false;
            }
            Tile tile = this.TileGrid[x, y].Tile;
            tile.Entity.Destroy();
            this.TileGrid.Remove(tile.X, tile.Y);
            return true;
        }

        void ApplyTentativeGridChanges()
        {
            this.TileBuffsGrid = this.tentativeFarmGrid.TileBuffsGrid;
            foreach (var weakTile in this.tentativeFarmGrid.AppliedTileChanges)
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
                        this.AddTile(Tile.CreateTile(advancesFromTileType.Value, weakTile.X, weakTile.Y, true, weakTile.TileType));
                    }
                    else
                    {
                        this.AddTile(Tile.CreateTile(weakTile.TileType, weakTile.X, weakTile.Y));
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
                foreach (var tileEntity in this.TileGrid.AllValues())
                {
                    tileEntity.Enabled = true;
                }
            }
        }

        public override void OnDisabled()
        {
            // disable all tiles
            foreach (var tileEntity in this.TileGrid.AllValues())
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

        public override void ProcessEvent(TentativeFarmGridOnEvent e)
        {
            this.Enabled = false;
            this.tentativeFarmGrid.Enabled = true;
        }

        public override void ProcessEvent(TentativeFarmGridOffEvent e)
        {
            // apply tentative changes
            if (e.ApplyChanges)
            {
                this.ApplyTentativeGridChanges();
            }
            // call this to truly reenable the grid
            OnEnabled();
        }

        public override void ProcessEvent(NewSeasonEvent e)
        {
            foreach (var tileEntity in this.TileGrid.AllValues())
            {
                var tile = tileEntity.Tile;
                if (!tile.IsPlaceable())
                {
                    this.RemoveTile(tile.X, tile.Y);
                }
            }
        }

        #endregion
    }
}