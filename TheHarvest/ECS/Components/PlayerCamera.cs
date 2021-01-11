using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace TheHarvest.ECS.Components
{
    public class PlayerCamera : Component, IUpdatable
    {
        VirtualIntegerAxis xAxisInput = new VirtualIntegerAxis();
        VirtualIntegerAxis yAxisInput = new VirtualIntegerAxis();
        VirtualButton zoomInInput = new VirtualButton();
        VirtualButton zoomOutInput = new VirtualButton();
        float moveSpeed = 20;
        float zoomSpeed = 20;

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            this.SetupInput();
        }

        private void SetupInput()
        {
            this.xAxisInput.AddKeyboardKeys(VirtualInput.OverlapBehavior.CancelOut, Keys.Left, Keys.Right);
            this.yAxisInput.AddKeyboardKeys(VirtualInput.OverlapBehavior.CancelOut, Keys.Up, Keys.Down);
            this.zoomInInput.AddKeyboardKey(Keys.Add);
            this.zoomOutInput.AddKeyboardKey(Keys.Subtract);
        }

        public void Update()
        {
            this.UpdateMovement();
        }

        private void UpdateMovement()
        {
            var motion = Vector2.Zero;
            if (xAxisInput.Value < 0) {
                motion.X = -moveSpeed;
            }
            else if (xAxisInput > 0) {
                motion.X = moveSpeed;
            }
            if (yAxisInput.Value < 0) {
                motion.Y = -moveSpeed;
            }
            else if (yAxisInput > 0) {
                motion.Y = moveSpeed;
            }
            this.Entity.Position += motion;
        }
    }
}