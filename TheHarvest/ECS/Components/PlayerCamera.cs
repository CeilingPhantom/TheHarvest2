using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using System;

namespace TheHarvest.ECS.Components
{
    public class PlayerCamera : Component, IUpdatable
    {
        VirtualIntegerAxis xAxisInput = new VirtualIntegerAxis();
        VirtualIntegerAxis yAxisInput = new VirtualIntegerAxis();
        VirtualButton zoomInInput = new VirtualButton();
        VirtualButton zoomOutInput = new VirtualButton();
        float moveSpeedParam = 0;
        float moveSpeedParamShift = 0.01f;
        float moveSpeedMultiplier = 10;
        float zoomSpeed = 0.01f;
        Camera camera;

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            this.SetupInput();
            this.camera = this.Entity.Scene.Camera;
        }

        void SetupInput()
        {
            this.xAxisInput.AddKeyboardKeys(VirtualInput.OverlapBehavior.CancelOut, Keys.Left, Keys.Right);
            this.yAxisInput.AddKeyboardKeys(VirtualInput.OverlapBehavior.CancelOut, Keys.Up, Keys.Down);
            this.zoomInInput.AddKeyboardKey(Keys.Add);
            this.zoomOutInput.AddKeyboardKey(Keys.Subtract);
        }

        public void Update()
        {
            this.UpdateMovement();
            this.UpdateZoom();
        }

        void UpdateMovement()
        {
            var motion = Vector2.Zero;
            var moveSpeed = CalcMoveSpeed();
            if (xAxisInput.Value < 0)
                motion.X = -moveSpeed;
            else if (xAxisInput > 0)
                motion.X = moveSpeed;
            if (yAxisInput.Value < 0)
                motion.Y = -moveSpeed;
            else if (yAxisInput > 0)
                motion.Y = moveSpeed;
            this.Entity.Position += motion;
        }

        float CalcMoveSpeed()
        {
            // rate of change in speed slows down significantly as camera approaches min/max zoom
            return (float) ((2 * Math.Atan(this.moveSpeedParam) / Math.PI) + 1) * this.moveSpeedMultiplier;
        }

        void UpdateZoom()
        {
            if (zoomInInput.IsDown)
            {
                this.camera.ZoomIn(zoomSpeed);
                if (this.camera.Zoom != 1f)
                    this.moveSpeedParam -= this.moveSpeedParamShift;
            }
            else if (zoomOutInput.IsDown)
            {
                this.camera.ZoomOut(zoomSpeed);
                if (this.camera.Zoom != -1f)
                    this.moveSpeedParam += this.moveSpeedParamShift;
            }
        }

        public void SetZoom(float zoom)
        {
            var currZoom = this.camera.Zoom;
            this.camera.SetZoom(zoom);
            var diff = currZoom - this.camera.Zoom;
            this.moveSpeedParamShift += diff / this.zoomSpeed * this.moveSpeedParamShift;
        }
    }
}