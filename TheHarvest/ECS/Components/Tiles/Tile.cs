using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Nez;
using Nez.Sprites;
using Nez.Textures;

using TheHarvest.ECS.Components.Farm;

namespace TheHarvest.ECS.Components.Tiles
{
    public enum TileType : byte
    {
        Dirt,
        Grass,
        // intermediate advancing tiles
        Construct,
        Field,
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

        static readonly string[] AdvancesFromField = new string[] { "Blueberry", "Carrot", "Potato", "Strawberry", "Wheat" };
        static readonly string[] AdvancesFromConstruct = new string[] { "Chicken", "Pig", "Greenhouse", "Shed", "Silo" };

        public Tile(TileType type, int x, int y, int cost=0, bool isAdvancing=false, TileType advancingType=0, float cycleTime=0)
        {
            this.TileType = type;
            this.X = x;
            this.Y = y;
            this.Cost = cost;
            this.IsAdvancing = isAdvancing;
            this.AdvancingType = advancingType;
            this.CycleTime = cycleTime;
        }

        public static Tile CreateTile(TileType type, int x, int y, bool isAdvancing=false, TileType advancingType=0, float cycleTime=0)
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

        public static int TileTypeLevel(TileType tileType)
        {
            var r = Regex.Match(tileType.ToString(), @"[0-9]$");
            if (r.Success)
                return int.Parse(r.Value);
            return 0;
        }

        public static bool AreSameBaseTileType(TileType tileTypeA, TileType tileTypeB)
        {
            return Tile.BaseTileType(tileTypeA) == Tile.BaseTileType(tileTypeB);
        }

        public static TileType? AdvancesFrom(TileType tileType)
        {
            if (Tile.AdvancesFromField.Contains(Tile.BaseTileType(tileType)))
                return TileType.Field;
            if (Tile.AdvancesFromConstruct.Contains(Tile.BaseTileType(tileType)))
                return TileType.Construct;
            return null;
        }

        public static int GetCost(TileType tileType)
        {
            if (!Tile.tileCosts.ContainsKey(tileType)) {
                Tile.tileCosts[tileType] = Tile.CreateTile(tileType, 0, 0).Cost;
            }
            return Tile.tileCosts[tileType];
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            this.SpriteAnimator = this.Entity.GetComponent<SpriteAnimator>();
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
                this.AdvanceTile();
        }

        void UpdateCycleTime()
        {
            this.CycleTime += Time.DeltaTime;
            if (!this.IsAdvancing)
                this.CycleTime %= Tile.CycleDuration;
        }

        /// <summary>
        /// if calling this in Update(), make sure this is the last thing done before it returns
        /// </summary>
        protected virtual void AdvanceTile()
        {
            this.FarmGrid.ReplaceTile(Tile.CreateTile(this.AdvancingType, this.X, this.Y));
        }

        public int CompareTo(Tile other)
        {
            if (other == null)
                return 1;
            var TypeCmp = this.TileType.CompareTo(other.TileType);
            if (TypeCmp != 0)
                return TypeCmp;
            var XCmp = this.X.CompareTo(other.X);
            if (XCmp != 0)
                return XCmp;
            var YCmp = this.Y.CompareTo(other.Y);
            if (YCmp != 0)
                return YCmp;
            var CycleTimeCmp = this.CycleTime.CompareTo(other.CycleTime);
            if (CycleTimeCmp != 0)
                return CycleTimeCmp;
            if (this.IsAdvancing != other.IsAdvancing)
                return this.IsAdvancing.CompareTo(other.IsAdvancing);
            return this.AdvancingType.CompareTo(other.AdvancingType);
        }
    }
}