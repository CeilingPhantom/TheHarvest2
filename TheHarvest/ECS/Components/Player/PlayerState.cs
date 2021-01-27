using System;
using Nez;

using TheHarvest.Events;

namespace TheHarvest.ECS.Components.Player
{
    public class PlayerState : EventSubscribingComponent
    {
        static readonly Lazy<PlayerState> lazy = new Lazy<PlayerState>(() => new PlayerState());
        public static PlayerState Instance => lazy.Value;

        public static readonly int ChunkSize = 11;

        public int Money { get; private set; }
        public float TimeOfDay { get; private set; }  // time in seconds
        public byte Day { get; private set; }
        public byte Season { get; private set; }
        public byte Year { get; private set; }

        PlayerState() : base()
        {
            EventManager.Instance.SubscribeTo<AddMoneyEvent>(this);
        }

        public void LoadFromBytes(byte[] bytes)
        {
            this.Money = BitConverter.ToInt32(bytes, 0);
            this.TimeOfDay = BitConverter.ToSingle(bytes, 4);
            this.Day = bytes[8];
            this.Season = bytes[9];
            this.Year = bytes[10];
        }

        public byte[] ToBytes()
        {
            var bytes = new byte[PlayerState.ChunkSize];
            BitConverter.GetBytes(this.Money).CopyTo(bytes, 0);
            BitConverter.GetBytes(this.TimeOfDay).CopyTo(bytes, 4);
            bytes[8] = this.Day;
            bytes[9] = this.Season;
            bytes[10] = this.Year;
            return bytes;
        }

        public override void Update()
        {
            base.Update();
            this.TimeOfDay += Time.DeltaTime;
        }

        #region Event Processing

        #endregion
    }
}