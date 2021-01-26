using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;

using TheHarvest.ECS.Components;
using TheHarvest.Events;

namespace TheHarvest.UI
{
    public class TileWindow : Window
    {
        PlayerCamera playerCamera = PlayerCamera.Instance;
        
        public TileWindow() : base("hello world", new WindowStyle())
        {
            this.DebugAll();
            this.SetPosition(100, 100);
            this.PadTop(10);
        }
    }
}