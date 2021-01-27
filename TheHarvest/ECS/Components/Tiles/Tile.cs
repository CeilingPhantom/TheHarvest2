using System;
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
        public static readonly int ChunkSize = 15;
        public static readonly float Size = 32;

        public FarmGrid FarmGrid { get; protected internal set; }
        public TileType TileType { get; }
        public int X { get; internal set; }
        public int Y { get; internal set; }
        public float CycleTime { get; internal set; }  // not for tile animation - that is managed by the sprite animator
        public bool IsAdvancing { get; internal set; }
        public TileType AdvancingType { get; internal set; }

        protected SpriteAnimator SpriteAnimator;
        static readonly string defaultAnimationName = "default";

        public Tile(TileType type, int x, int y, float cycleTime=0, bool isAdvancing=false, TileType advancingType=0)
        {
            this.TileType = type;
            this.X = x;
            this.Y = y;
            this.CycleTime = cycleTime;
            this.IsAdvancing = isAdvancing;
            this.AdvancingType = advancingType;
        }

        public static Tile CreateTile(TileType type, int x, int y, float cycleTime=0, bool isAdvancing=false, TileType advancingType=0)
        {
            Tile tile;
            switch(type)
            {
                case TileType.Dirt:
                    tile = new DirtTile(x, y, cycleTime, isAdvancing, advancingType);
                    break;
                case TileType.Grass:
                    tile = new GrassTile(x, y, cycleTime, isAdvancing, advancingType);
                    break;
                case TileType.Blueberry1:
                    tile = new Blueberry1Tile(x, y, cycleTime, isAdvancing, advancingType);
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
            var cycleTime = BitConverter.ToSingle(byteInput, 9);
            var isAdvancing = BitConverter.ToBoolean(byteInput, 13);
            var advancingType = (TileType) byteInput[14];
            return CreateTile(type, x, y, cycleTime, isAdvancing, advancingType);
        }

        public byte[] ToBytes()
        {
            var bytes = new byte[Tile.ChunkSize];
            bytes[0] = (byte) this.TileType;
            BitConverter.GetBytes(this.X).CopyTo(bytes, 1);
            BitConverter.GetBytes(this.Y).CopyTo(bytes, 5);
            BitConverter.GetBytes(this.CycleTime).CopyTo(bytes, 9);
            BitConverter.GetBytes(this.IsAdvancing).CopyTo(bytes, 13);
            bytes[14] = (byte) this.AdvancingType;
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
            this.CycleTime += Time.DeltaTime;
        }

        /// <summary>
        /// if calling this in Update(), make sure this is the last thing done before it returns
        /// </summary>
        protected virtual void AdvanceTile()
        {
            this.FarmGrid.ReplaceTile(this, this.AdvancingType);
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