using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Nez;
using Nez.Sprites;
using Nez.Textures;

using TheHarvest.ECS.Components.Farm;
using TheHarvest.FileManagers;

namespace TheHarvest.ECS.Components.Tiles
{
    public enum TileType : byte
    {
        // utility tiletypes for tile selection ui - these cannot be created
        Destruct,
        Upgrade,
        Reset,
        ResetAll,
        Undo,
        Redo,
        
        // basic, nothing-special-about-them tiles
        Dirt,
        Grass,
        // intermediate advancing tiles
        Field,
        Construct,
        // tiles that advance from field
        // crops
        Blueberry1,
        Blueberry2,
        Blueberry3,
        Carrot1,
        Carrot2,
        Carrot3,
        Potato1,
        Potato2,
        Potato3,
        Strawberry1,
        Strawberry2,
        Strawberry3,
        Wheat1,
        Wheat2,
        Wheat3,

        // tiles that advances from construct
        // animals
        Chicken1,
        Chicken2,
        Chicken3,
        Pig1,
        Pig2,
        Pig3,
        // structures
        Greenhouse1,
        Greenhouse2,
        Greenhouse3,
        Shed1,
        Shed2,
        Shed3,
        Silo1,
        Silo2,
        Silo3,
    }
    
    public abstract class Tile : Component, IUpdatable, IComparable<Tile>
    {
        public static readonly int ChunkSize = sizeof(TileType) * 2 + sizeof(int) * 2 + sizeof(float) + sizeof(bool);  // 15
        public static readonly float Size = 32;

        static Dictionary<TileType, int> tileCosts = new Dictionary<TileType, int>();

        public FarmGrid FarmGrid { get; protected internal set; }
        public TileType TileType { get; }
        public int X { get; protected internal set; }
        public int Y { get; protected internal set; }
        public int Cost { get; private set; }
        public bool IsAdvancing { get; protected set; }
        public TileType AdvancingType { get; protected set; }
        public float CycleTime { get; protected set; }  // not for tile animation - that is managed by the sprite animator

        public static readonly float CycleDuration = 5f;  // TODO replace

        protected SpriteAnimator SpriteAnimator;
        static readonly string defaultAnimationName = "default";

        public Tile(TileType type, int x, int y, int cost=0, bool isAdvancing=false, TileType advancingType=TileType.Dirt, float cycleTime=0)
        {
            this.TileType = type;
            this.X = x;
            this.Y = y;
            this.Cost = cost;
            this.IsAdvancing = isAdvancing;
            this.AdvancingType = advancingType;
            this.CycleTime = cycleTime;
        }

        public static Tile CreateTile(TileType type, int x, int y, bool isAdvancing=false, TileType advancingType=TileType.Dirt, float cycleTime=0)
        {
            Tile tile;
            switch(type)
            {
                case TileType.Dirt:
                    tile = new DirtTile(x, y, isAdvancing, advancingType, cycleTime);
                    break;
                case TileType.Grass:
                    tile = new GrassTile(x, y, isAdvancing, advancingType, cycleTime);
                    break;
                case TileType.Field:
                    tile = new FieldTile(x, y, isAdvancing, advancingType, cycleTime);
                    break;
                case TileType.Blueberry1:
                    tile = new Blueberry1Tile(x, y, isAdvancing, advancingType, cycleTime);
                    break;
                case TileType.Blueberry2:
                    tile = new Blueberry2Tile(x, y, isAdvancing, advancingType, cycleTime);
                    break;
                case TileType.Blueberry3:
                    tile = new Blueberry3Tile(x, y, isAdvancing, advancingType, cycleTime);
                    break;
                case TileType.Carrot1:
                    tile = new Carrot1Tile(x, y, isAdvancing, advancingType, cycleTime);
                    break;
                case TileType.Carrot2:
                    tile = new Carrot2Tile(x, y, isAdvancing, advancingType, cycleTime);
                    break;
                case TileType.Carrot3:
                    tile = new Carrot3Tile(x, y, isAdvancing, advancingType, cycleTime);
                    break;
                case TileType.Potato1:
                    tile = new Potato1Tile(x, y, isAdvancing, advancingType, cycleTime);
                    break;
                case TileType.Potato2:
                    tile = new Potato2Tile(x, y, isAdvancing, advancingType, cycleTime);
                    break;
                case TileType.Potato3:
                    tile = new Potato3Tile(x, y, isAdvancing, advancingType, cycleTime);
                    break;
                case TileType.Strawberry1:
                    tile = new Strawberry1Tile(x, y, isAdvancing, advancingType, cycleTime);
                    break;
                case TileType.Strawberry2:
                    tile = new Strawberry2Tile(x, y, isAdvancing, advancingType, cycleTime);
                    break;
                case TileType.Strawberry3:
                    tile = new Strawberry3Tile(x, y, isAdvancing, advancingType, cycleTime);
                    break;
                case TileType.Wheat1:
                    tile = new Wheat1Tile(x, y, isAdvancing, advancingType, cycleTime);
                    break;
                case TileType.Wheat2:
                    tile = new Wheat2Tile(x, y, isAdvancing, advancingType, cycleTime);
                    break;
                case TileType.Wheat3:
                    tile = new Wheat3Tile(x, y, isAdvancing, advancingType, cycleTime);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }
            return tile;
        }

        public static Tile CreateTile(byte[] byteInput)
        {
            var type = (TileType) byteInput[0];
            var x = BitConverter.ToInt32(byteInput, 1);
            var y = BitConverter.ToInt32(byteInput, 5);
            var isAdvancing = BitConverter.ToBoolean(byteInput, 9);
            var advancingType = (TileType) byteInput[10];
            var cycleTime = BitConverter.ToSingle(byteInput, 11);
            return CreateTile(type, x, y, isAdvancing, advancingType, cycleTime);
        }

        public byte[] ToBytes()
        {
            var bytes = new byte[Tile.ChunkSize];
            bytes[0] = (byte) this.TileType;
            BitConverter.GetBytes(this.X).CopyTo(bytes, 1);
            BitConverter.GetBytes(this.Y).CopyTo(bytes, 5);
            BitConverter.GetBytes(this.IsAdvancing).CopyTo(bytes, 9);
            bytes[10] = (byte) this.AdvancingType;
            BitConverter.GetBytes(this.CycleTime).CopyTo(bytes, 11);
            return bytes;
        }

        public static string BaseTileType(TileType tileType)
        {
            return Regex.Match(tileType.ToString(), @"[a-zA-z]+").Value;
        }

        public static int GetTileTypeLevel(TileType tileType)
        {
            var r = Regex.Match(tileType.ToString(), @"[0-9]$");
            return r.Success ? int.Parse(r.Value) : 0;
        }

        public static bool GetUpgradedTileType(TileType tileType, out TileType upgradedTiletype)
        {
            if (tileType > TileType.Field && Tile.GetTileTypeLevel(tileType) < 3)
            {
                upgradedTiletype = Tile.GetCorrespondingTileType(Tile.BaseTileType(tileType), Tile.GetTileTypeLevel(tileType) + 1);
                return true;
            }
            upgradedTiletype = tileType;
            return false;
        }

        static TileType GetCorrespondingTileType(string baseTileType, int level)
        {
            var tileTypeStr = $"{baseTileType}{level}";
            return (TileType) Enum.Parse(typeof(TileType), tileTypeStr);
        }

        public static bool AreSameBaseTileType(TileType tileTypeA, TileType tileTypeB)
        {
            return Tile.BaseTileType(tileTypeA) == Tile.BaseTileType(tileTypeB);
        }

        public static TileType GetFutureTileType(Tile tile)
        {
            // use the tile that it will advance into, if it exists
            return tile.IsAdvancing ? tile.AdvancingType : tile.TileType;
        }

        public static TileType? AdvancesFrom(TileType tileType)
        {
            if (tileType >= TileType.Blueberry1 && tileType <= TileType.Wheat3)
            {
                return TileType.Field;
            }
            if (tileType >= TileType.Chicken1 && tileType <= TileType.Silo3)
            {
                return TileType.Construct;
            }
            return null;
        }

        public static int GetCost(TileType tileType)
        {
            if (!Tile.tileCosts.ContainsKey(tileType))
            {
                Tile.tileCosts[tileType] = Tile.CreateTile(tileType, 0, 0).Cost;
            }
            return Tile.tileCosts[tileType];
        }

        public static bool GetUpgradeCost(TileType currTiletype, TileType upgradedTiletype, out int upgradeCost)
        {
            upgradeCost = 0;
            if (Tile.AreSameBaseTileType(currTiletype, upgradedTiletype))
            {
                if (Tile.GetTileTypeLevel(currTiletype) < Tile.GetTileTypeLevel(upgradedTiletype))
                {
                    upgradeCost = Tile.GetCost(upgradedTiletype) - Tile.GetCost(currTiletype);
                    return true;
                }
            }
            return false;
        }

        public override void OnAddedToEntity()
        {
            this.SpriteAnimator = this.Entity.GetComponent<SpriteAnimator>();
            if (TilesetSpriteManager.Instance.HasAnimation(this.TileType))
            {
                this.SetAnimation(TilesetSpriteManager.Instance.GetAnimation(this.TileType));
                this.PlayAnimation();
            }
            else
            {
                this.SetSprite(TilesetSpriteManager.Instance.GetSprite(this.TileType));
            }
        }

        protected void SetSprite(Sprite sprite)
        {
            this.SpriteAnimator.SetSprite(sprite);
        }

        protected void SetAnimation(SpriteAnimation animation)
        {
            this.SpriteAnimator.AddAnimation(Tile.defaultAnimationName, animation);
        }

        protected void PlayAnimation()
        {
            this.SpriteAnimator.Play(Tile.defaultAnimationName);
        }

        public virtual void Update()
        {
            this.UpdateCycleTime();
            // if tile can advance
            if (this.CycleTime > Tile.CycleDuration)
            {
                this.AdvanceTile();
            }
        }

        void UpdateCycleTime()
        {
            this.CycleTime += Time.DeltaTime;
            if (!this.IsAdvancing)
            {
                this.CycleTime %= Tile.CycleDuration;
            }
        }

        /// <summary>
        /// if calling this in Update(), make sure this is the last thing done before it returns
        /// </summary>
        protected virtual void AdvanceTile()
        {
            this.FarmGrid.AddTile(Tile.CreateTile(this.AdvancingType, this.X, this.Y));
        }

        public int CompareTo(Tile other)
        {
            if (other == null)
            {
                return 1;
            }
            var TypeCmp = this.TileType.CompareTo(other.TileType);
            if (TypeCmp != 0)
            {
                return TypeCmp;
            }
            var XCmp = this.X.CompareTo(other.X);
            if (XCmp != 0)
            {
                return XCmp;
            }
            var YCmp = this.Y.CompareTo(other.Y);
            if (YCmp != 0)
            {
                return YCmp;
            }
            var CycleTimeCmp = this.CycleTime.CompareTo(other.CycleTime);
            if (CycleTimeCmp != 0)
            {
                return CycleTimeCmp;
            }
            if (this.IsAdvancing != other.IsAdvancing)
            {
                return this.IsAdvancing.CompareTo(other.IsAdvancing);
            }
            return this.AdvancingType.CompareTo(other.AdvancingType);
        }
    }
}