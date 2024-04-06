using System.Numerics;
using ZeroElectric.Vinculum;

namespace WoopWoop.UI
{
    public class ColorPicker : Component
    {
        public Rectangle bounds;
        public Color color = Raylib.RED;

        public override void Update(float deltaTime)
        {
            bounds = new Rectangle(transform.Position.X, transform.Position.Y, transform.Scale.X, transform.Scale.Y);
            RayGui.GuiColorPicker(bounds, "", ref color);
        }
    }
}