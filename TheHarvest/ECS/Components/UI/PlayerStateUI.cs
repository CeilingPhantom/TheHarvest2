using System;
using Nez.UI;

using TheHarvest.ECS.Components.Player;
using TheHarvest.UI;
using TheHarvest.Util.Input;

namespace TheHarvest.ECS.Components.UI
{
    public class PlayerStateUI : BaseUI
    {
        PlayerCamera playerCamera = PlayerCamera.Instance;
        PlayerState playerState = PlayerState.Instance;
        BaseWindow window;
        
        Label moneyLabel;
        Label timeLabel;

        static readonly int inputPriority = 90;

        public PlayerStateUI() : base()
        {
            InputManager.Instance.Register(this, PlayerStateUI.inputPriority);

            this.window = this.Stage.AddElement(new BaseWindow());
            this.AddMoneyLabel();
            this.AddTimeLabel();
            this.window.SetMovable(false);
            this.window.SetPosition(0, Stage.GetHeight());
            this.window.Pack();
            this.window.SetWidth(Stage.GetWidth());
            this.window.Align((int) Align.Right);
            this.window.DebugAll();
        }

        void AddMoneyLabel()
        {
            this.moneyLabel = new Label($"${this.playerState.Money}").SetAlignment(Align.Center);
            this.window.Add(this.moneyLabel).SetPadLeft(20).SetPadRight(20).SetPrefWidth(100);
        }

        void UpdateMoneyLabel()
        {
            this.moneyLabel.SetText($"${this.playerState.Money}");
        }

        void AddTimeLabel()
        {
            this.timeLabel = new Label($"{Math.Floor(this.playerState.TimeOfDay)}").SetAlignment(Align.Center);
            this.window.Add(this.timeLabel).SetPadLeft(20).SetPadRight(20).SetPrefWidth(100);
        }

        void UpdateTimeLabel()
        {
            this.timeLabel.SetText($"{Math.Floor(this.playerState.TimeOfDay)}");
        }

        public override void Update()
        {
            if (InputManager.Instance.CanAcceptInput(PlayerStateUI.inputPriority))
            {
                base.Update();
            }
            UpdateMoneyLabel();
            UpdateTimeLabel();
        }

        #region IInputable

        public override bool InputCollision()
        {
            var mousePos = this.Stage.GetMousePosition();
            if (mousePos.X >= this.window.GetX() && 
                mousePos.Y >= this.window.GetY() && 
                mousePos.X < this.window.GetX() + this.window.GetWidth() && 
                mousePos.Y < this.window.GetY() + this.window.GetHeight())
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}