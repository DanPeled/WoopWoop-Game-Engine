using System.Numerics;
using ZeroElectric.Vinculum;

namespace WoopWoop.UI
{
    public class ColorPicker : UIBoundsComponent
    {
        public Color color = Raylib.RED;

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            RayGui.GuiColorPicker(bounds, "", ref color);
        }
    }
}