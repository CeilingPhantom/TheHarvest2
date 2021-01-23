using System;
using Nez;
using Nez.Sprites;

namespace TheHarvest.ECS.Components
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

        public Farm Farm { get; protected internal set; }
        public TileType Type { get; }
        public int X { get; internal set; }
        public int Y { get; internal set; }
        public float CycleTime { get; internal set; }  // not for tile animation - that is managed by the sprite animator
        public bool IsAdvancing { get; internal set; }
        public TileType AdvancingType { get; internal set; }

        protected SpriteAnimator SpriteAnimator;

        public Tile(TileType type, int x, int y, float cycleTime=0, bool isAdvancing=false, TileType advancingType=0)
        {
            this.Type = type;
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
            bytes[0] = (byte) this.Type;
            BitConverter.GetBytes(this.X).CopyTo(bytes, 1);
            BitConverter.GetBytes(this.Y).CopyTo(bytes, 5);
            BitConverter.GetBytes(this.CycleTime).CopyTo(bytes, 9);
            BitConverter.GetBytes(this.IsAdvancing).CopyTo(bytes, 13);
            bytes[14] = (byte) this.AdvancingType;
            return bytes;
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            this.SpriteAnimator = this.Entity.GetComponent<SpriteAnimator>();
        }

        public virtual void Update()
        {
            //this.CycleTime += Time.DeltaTime;
        }

        protected virtual void AdvanceTile()
        {

        }

        public int CompareTo(Tile other)
        {
            if (other == null)
                return 1;
            var TypeCmp = this.Type.CompareTo(other.Type);
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