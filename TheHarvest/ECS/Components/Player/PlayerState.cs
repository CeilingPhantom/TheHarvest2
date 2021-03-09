using System;
using Nez;

using TheHarvest.Events;

namespace TheHarvest.ECS.Components.Player
{
    public class PlayerState : EventSubscribingComponent
    {
        [Flags]
        public enum Seasons : byte
        {
            Spring = 1,
            Summer = 1 << 1,
            Fall = 1 << 2,
            Winter = 1 << 3,
        }

        public static int NSeasons = Enum.GetNames(typeof(Seasons)).Length;

        static readonly Lazy<PlayerState> lazy = new Lazy<PlayerState>(() => new PlayerState());
        public static PlayerState Instance => lazy.Value;

        public static readonly int ChunkSize = 11;

        static readonly float TimeInDay = 5;  // in seconds
        static readonly byte DaysInSeason = 2;

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
                if (this.isTentativeState)
                {
                    this.tentativeMoney = value;
                }
                else
                {
                    this.money = value;
                }
            }
        }
        public float TimeOfDay { get; private set; }  // time in seconds
        public byte Day { get; private set; }
        public Seasons Season { get; private set; }
        public byte Year { get; private set; }

        PlayerState() : base()
        {
            EventManager.Instance.SubscribeTo<AddMoneyEvent>(this);
            EventManager.Instance.SubscribeTo<TentativeFarmGridOnEvent>(this);
            EventManager.Instance.SubscribeTo<TentativeFarmGridOffEvent>(this);
            
            this.money = 1000;
            this.Season = Seasons.Spring;
        }

        public void LoadFromBytes(byte[] bytes)
        {
            this.money = BitConverter.ToInt32(bytes, 0);
            this.TimeOfDay = BitConverter.ToSingle(bytes, 4);
            this.Day = bytes[8];
            this.Season = (Seasons) bytes[9];
            this.Year = bytes[10];
        }

        public byte[] ToBytes()
        {
            var bytes = new byte[PlayerState.ChunkSize];
            BitConverter.GetBytes(this.money).CopyTo(bytes, 0);
            BitConverter.GetBytes(this.TimeOfDay).CopyTo(bytes, 4);
            bytes[8] = this.Day;
            bytes[9] = (byte) this.Season;
            bytes[10] = this.Year;
            return bytes;
        }

        public override void Update()
        {
            base.Update();
            if (!this.isTentativeState)
            {
                this.UpdateTime();
            }
        }

        void UpdateTime()
        {
            this.TimeOfDay += Time.DeltaTime;
            if (this.TimeOfDay >= PlayerState.TimeInDay)
            {
                this.TimeOfDay -= PlayerState.TimeInDay;
                this.Day += 1;
                EventManager.Instance.Publish(new NewDayEvent());
            }
            if (this.Day >= PlayerState.DaysInSeason)
            {
                this.Day = 0;
                this.Season = (Seasons) ((byte) this.Season << 1);
                EventManager.Instance.Publish(new NewSeasonEvent());
            }
            if ((byte) this.Season > (1 << PlayerState.NSeasons))
            {
                this.Season = 0;
                this.Year += 1;
                EventManager.Instance.Publish(new NewYearEvent());
            }
        }

        #region Event Processing

        public override void ProcessEvent(AddMoneyEvent e)
        {
            this.Money += e.Amount;
        }

        public override void ProcessEvent(TentativeFarmGridOnEvent e)
        {
            this.isTentativeState = true;
            this.tentativeMoney = this.money;
        }

        public override void ProcessEvent(TentativeFarmGridOffEvent e)
        {
            this.isTentativeState = false;
            if (e.ApplyChanges)
            {
                this.money = this.tentativeMoney;
            }
        }

        #endregion
    }
}