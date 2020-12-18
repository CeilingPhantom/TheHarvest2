using System;
using Nez;

namespace TheHarvest.ECS.Components
{
    public enum TileType : byte
    {
        Dirt,
    }
    
    public abstract class Tile : Component, IUpdatable
    {
        // tile type - 1 byte
        // x pos - int, 4 bytes
        // y pos - int, 4 bytes
        // current cycle time - float, 4 bytes
        // will advance into another tile - 1 byte
        // advancing tile type - 1 byte
        public static readonly int ChunkSize = 15;

        public Farm Farm { get; }
        public TileType Type { get; }
        public int X { get; }
        public int Y { get; }
        public float CycleTime { get; internal set; }
        public bool IsAdvancing { get; internal set; }
        public TileType AdvancingType { get; internal set; }

        public Tile(Farm farm, TileType type, int x, int y, float cycleTime=0, bool isAdvancing=false, TileType advancingType=0)
        {
            this.Farm = farm;
            this.Type = type;
            this.X = x;
            this.Y = y;
            this.CycleTime = cycleTime;
            this.IsAdvancing = isAdvancing;
            this.AdvancingType = advancingType;
        }

        public static Tile CreateTile(Farm farm, TileType type, int x, int y, float cycleTime=0, bool isAdvancing=false, TileType advancingType=0)
        {
            return type switch
            {
                TileType.Dirt   => new DirtTile(farm, x, y, cycleTime, isAdvancing, advancingType),
                _               => throw new ArgumentOutOfRangeException(nameof(type)),
            };
        }

        public static Tile CreateTile(Farm farm, byte[] byteInput)
        {
            var type = (TileType) byteInput[0];
            var x = BitConverter.ToInt32(byteInput, 1);
            var y = BitConverter.ToInt32(byteInput, 5);
            var cycleTime = BitConverter.ToSingle(byteInput, 9);
            var isAdvancing = BitConverter.ToBoolean(byteInput, 13);
            var advancingType = (TileType) byteInput[14];
            return CreateTile(farm, type, x, y, cycleTime, isAdvancing, advancingType);
        }

        public byte[] ToBytes()
        {
            var bytes = new byte[ChunkSize];
            bytes[0] = (byte) Type;
            BitConverter.GetBytes(X).CopyTo(bytes, 1);
            BitConverter.GetBytes(Y).CopyTo(bytes, 5);
            BitConverter.GetBytes(CycleTime).CopyTo(bytes, 9);
            BitConverter.GetBytes(IsAdvancing).CopyTo(bytes, 13);
            bytes[14] = (byte) AdvancingType;
            return bytes;
        }

        public virtual void Update()
        {
            CycleTime += Time.DeltaTime;
        }

        protected virtual void AdvanceTile()
        {

        }
    }
}