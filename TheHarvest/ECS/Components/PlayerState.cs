using System;
using Nez;

namespace TheHarvest.ECS.Components
{
    public class PlayerState : Component, IUpdatable
    {
        static readonly Lazy<PlayerState> lazy = new Lazy<PlayerState>(() => new PlayerState());
        public static PlayerState Instance = lazy.Value;

        public static readonly int ChunkSize = 11;

        public int Money { get; private set; }
        public float TimeDay { get; private set; }
        public byte Day { get; private set; }
        public byte Season { get; private set; }
        public byte Year { get; private set; }

        private PlayerState() : base()
        {}

        public void LoadFromBytes(byte[] bytes)
        {
            this.Money = BitConverter.ToInt32(bytes, 0);
            this.TimeDay = BitConverter.ToSingle(bytes, 4);
            this.Day = bytes[8];
            this.Season = bytes[9];
            this.Year = bytes[10];
        }

        public byte[] ToBytes()
        {
            var bytes = new byte[PlayerState.ChunkSize];
            BitConverter.GetBytes(this.Money).CopyTo(bytes, 0);
            BitConverter.GetBytes(this.TimeDay).CopyTo(bytes, 4);
            bytes[8] = this.Day;
            bytes[9] = this.Season;
            bytes[10] = this.Year;
            return bytes;
        }

        public void Update()
        {
            TimeDay += Time.DeltaTime;
        }
    }
}