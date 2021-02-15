using Microsoft.Xna.Framework;
using Nez.UI;

namespace TheHarvest.UI
{
    public class BaseWindow : Window
    {
        public BaseWindow() : base("", new WindowStyle())
        {
            this.SetBackground(new PrimitiveDrawable(Color.Aquamarine));
            this.Clear();
            this.Top().Left();
            this.Pad(10);
        }
    }
}