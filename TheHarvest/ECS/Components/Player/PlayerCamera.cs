using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using System;

namespace TheHarvest.ECS.Components
{
    public class PlayerCamera : Component, IUpdatable
    {
        static readonly Lazy<PlayerCamera> lazy = new Lazy<PlayerCamera>(() => new PlayerCamera());
        public static PlayerCamera Instance => lazy.Value;

        public Camera Camera;
        
        VirtualIntegerAxis xAxisInput = new VirtualIntegerAxis();
        VirtualIntegerAxis yAxisInput = new VirtualIntegerAxis();
        VirtualButton zoomInKeyInput = new VirtualButton();
        VirtualButton zoomOutKeyInput = new VirtualButton();
        
        int prevScrollValue;
        float scrollZoomMultiplier = 3;
        
        float moveSpeedParam = 0;
        float moveSpeedParamShift = 0.01f;
        float moveSpeedMultiplier = 10;
        float zoomSpeed = 0.01f;

        public RectangleF Bounds => this.Camera.Bounds;
        public float Width => this.Bounds.Width;
        public float Height => this.Bounds.Height;

        public int TopLeftTileX => (int) Math.Floor(this.Bounds.X / Tile.Size);
        public int TopLeftTileY => (int) Math.Floor(this.Bounds.Y / Tile.Size);
        float topTileOverflow => Math.Abs(this.Bounds.Y % Tile.Size);
        float leftTileOverflow => Math.Abs(this.Bounds.X % Tile.Size);
        public int WidthTiles => (int) Math.Ceiling((this.Width - this.leftTileOverflow) / Tile.Size) + 
                                 (this.leftTileOverflow == 0 ? 0 : 1) + 
                                 (this.TopLeftTileX < 0 ? 0 : 1);
        public int HeightTiles => (int) Math.Ceiling((this.Height - this.topTileOverflow) / Tile.Size) + 
                                  (this.topTileOverflow == 0 ? 0 : 1) + 
                                  (this.TopLeftTileY < 0 ? 0 : 1);

        PlayerCamera() : base()
        {}

        public override void Initialize()
        {
            base.Initialize();
            this.prevScrollValue = Mouse.GetState().ScrollWheelValue;
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            this.SetupInput();
            this.Camera = this.Entity.Scene.Camera;
        }

        void SetupInput()
        {
            this.xAxisInput.AddKeyboardKeys(VirtualInput.OverlapBehavior.CancelOut, Keys.Left, Keys.Right);
            this.yAxisInput.AddKeyboardKeys(VirtualInput.OverlapBehavior.CancelOut, Keys.Up, Keys.Down);
            this.zoomInKeyInput.AddKeyboardKey(Keys.Add);
            this.zoomOutKeyInput.AddKeyboardKey(Keys.Subtract);
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
            if (this.xAxisInput.Value < 0)
                motion.X = -moveSpeed;
            else if (this.xAxisInput > 0)
                motion.X = moveSpeed;
            if (this.yAxisInput.Value < 0)
                motion.Y = -moveSpeed;
            else if (this.yAxisInput > 0)
                motion.Y = moveSpeed;
            this.Entity.Position += motion;
        }

        float CalcMoveSpeed()
        {
            // rate of change in speed slows down significantly as camera approaches min/max zoom
            return (float) ((2 * Math.Atan(this.moveSpeedParam) / Math.PI) + 1) * this.moveSpeedMultiplier;
        }

        public void SetPosition(Vector2 position)
        {
            this.Entity.Position = position;
        }

        void UpdateZoom()
        {
            var mouseState = Mouse.GetState();
            if (this.zoomInKeyInput.IsDown || mouseState.ScrollWheelValue > this.prevScrollValue)
            {
                this.Camera.ZoomIn(this.zoomSpeed * (this.zoomInKeyInput.IsDown ? 1 : this.scrollZoomMultiplier));
                if (this.Camera.Zoom != 1f)
                    this.moveSpeedParam -= this.moveSpeedParamShift;
            }
            else if (this.zoomOutKeyInput.IsDown || mouseState.ScrollWheelValue < this.prevScrollValue)
            {
                this.Camera.ZoomOut(this.zoomSpeed * (this.zoomOutKeyInput.IsDown ? 1 : this.scrollZoomMultiplier));
                if (this.Camera.Zoom != -1f)
                    this.moveSpeedParam += this.moveSpeedParamShift;
            }
            this.prevScrollValue = mouseState.ScrollWheelValue;
        }

        public void SetZoom(float zoom)
        {
            var currZoom = this.Camera.Zoom;
            this.Camera.SetZoom(zoom);
            var diff = currZoom - this.Camera.Zoom;
            this.moveSpeedParamShift += diff / this.zoomSpeed * this.moveSpeedParamShift;
        }

        public Vector2 MouseToTilePosition()
        {
            var pos = Camera.MouseToWorldPoint();
            var tileX = (int) Math.Floor(pos.X / Tile.Size);
            var tileY = (int) Math.Floor(pos.Y / Tile.Size);
            return new Vector2(tileX, tileY);
        }
    }
}