using Nez.UI;

using TheHarvest.ECS.Components.Player;
using TheHarvest.UI;

namespace TheHarvest.ECS.Components.UI
{
    public class PlayerStateUI : BaseUI
    {
        PlayerCamera playerCamera = PlayerCamera.Instance;
        PlayerState playerState = PlayerState.Instance;
        BaseWindow window;
        Label moneyLabel;

        public PlayerStateUI() : base()
        {
            this.window = this.Stage.AddElement(new BaseWindow());
            this.AddMoneyLabel();
            this.window.SetMovable(false);
            this.window.SetPosition(Stage.GetWidth(), Stage.GetHeight());
            this.window.Pack();
        }

        void AddMoneyLabel()
        {
            this.moneyLabel = new Label($"${this.playerState.Money}");
            this.window.Add(this.moneyLabel);
        }

        void UpdateMoneyLabel()
        {
            this.moneyLabel.SetText($"${this.playerState.Money}");
        }

        public override void Update()
        {
            base.Update();

            UpdateMoneyLabel();
        }
    }
}