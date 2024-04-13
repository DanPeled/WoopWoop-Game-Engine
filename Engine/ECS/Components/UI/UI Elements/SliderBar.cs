using ZeroElectric.Vinculum;

namespace WoopWoop.UI
{
    public class SliderBar : Slider
    {
        public override unsafe void Render(sbyte* minBytePtr, sbyte* maxBytePtr, float* valuePtr)
        {
            RayGui.GuiSliderBar(bounds, (sbyte*)minBytePtr, (sbyte*)maxBytePtr, valuePtr, MinValue, MaxValue);
        }
    }
}