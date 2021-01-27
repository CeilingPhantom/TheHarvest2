using Microsoft.Xna.Framework;
using Nez.UI;

namespace TheHarvest.UI
{
    public class TileWindow : Window
    {
        public TileWindow() : base("hello world", new WindowStyle())
        {
            //this.DebugAll();
            this.SetBackground(new PrimitiveDrawable(Color.Aquamarine));
            //this.SetResizable(true);
            //this.SetResizeBorderSize(5);
            this.SetPosition(100, 100);
            this.Top().Left();
            this.Pad(10).PadTop(20);
        }
    }
}