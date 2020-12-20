using System;
using Nez;
using Nez.Sprites;

namespace TheHarvest.ECS.Components
{
    public enum TileType : byte
    {
        Dirt,
    }
    
    public abstract class Tile : Component, IUpdatable
    {
        public static readonly int ChunkSize = 15;

        public Farm Farm { get; protected internal set; }
        public TileType Type { get; }
        public int X { get; }
        public int Y { get; }
        public float CycleTime { get; internal set; }
        public bool IsAdvancing { get; internal set; }
        public TileType AdvancingType { get; internal set; }

        protected SpriteRenderer SpriteRenderer;

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
            this.SpriteRenderer = this.Entity.GetComponent<SpriteRenderer>();
        }

        public virtual void Update()
        {
            this.CycleTime += Time.DeltaTime;
        }

        protected virtual void AdvanceTile()
        {

        }
    }
}