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

        bool isTentativeState = false;

        int money;
        int tentativeMoney;
        public int Money
        {
            get
            {
                return this.isTentativeState ? this.tentativeMoney : this.money;
            }
            private set
            {
                if (this.isTentativeState) {
                    this.tentativeMoney = value;
                }
                else {
                    this.money = value;
                }
            }
        }
        public float TimeOfDay { get; private set; }  // time in seconds
        public byte Day { get; private set; }
        public byte Season { get; private set; }
        public byte Year { get; private set; }

        PlayerState() : base()
        {
            EventManager.Instance.SubscribeTo<AddMoneyEvent>(this);
            EventManager.Instance.SubscribeTo<EditFarmOnEvent>(this);
            EventManager.Instance.SubscribeTo<EditFarmOffEvent>(this);
            this.money = 15;
        }

        public void LoadFromBytes(byte[] bytes)
        {
            this.money = BitConverter.ToInt32(bytes, 0);
            this.TimeOfDay = BitConverter.ToSingle(bytes, 4);
            this.Day = bytes[8];
            this.Season = bytes[9];
            this.Year = bytes[10];
        }

        public byte[] ToBytes()
        {
            var bytes = new byte[PlayerState.ChunkSize];
            BitConverter.GetBytes(this.money).CopyTo(bytes, 0);
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

        public override void ProcessEvent(AddMoneyEvent e)
        {
            this.Money += e.Amount;
        }

        public override void ProcessEvent(EditFarmOnEvent e)
        {
            this.isTentativeState = true;
            this.tentativeMoney = this.money;
        }

        public override void ProcessEvent(EditFarmOffEvent e)
        {
            this.isTentativeState = false;
            if (e.ApplyChanges) {
                this.money = this.tentativeMoney;
            }
        }

        #endregion
    }
}