using System.Text;
using ZeroElectric.Vinculum;

namespace WoopWoop.UI
{
    public class Slider : Renderer
    {
        public Rectangle bounds;
        public float value;
        public string minText = "", maxText = "";
        public float minValue = 0, maxValue = 100;
        public Action<float> OnValueChange;
        public override void Update(float deltaTime)
        {
            float prevValue = value;
            bounds = new Rectangle(transform.Position.X, transform.Position.Y, transform.Scale.X, transform.Scale.Y);
            unsafe
            {
                byte[] minBytes = Encoding.ASCII.GetBytes(minText);
                byte[] maxBytes = Encoding.ASCII.GetBytes(maxText);

                fixed (byte* minBytePtr = minBytes, maxBytePtr = maxBytes)
                {
                    fixed (float* valuePtr = &value) // Allocate memory for the float value
                    {
                        Render((sbyte*)minBytePtr, (sbyte*)maxBytePtr, valuePtr);
                    }
                }
            }
            if (value != prevValue)
            {
                OnValueChange?.Invoke(value);
            }
        }
        public unsafe virtual void Render(sbyte* minBytePtr, sbyte* maxBytePtr, float* valuePtr)
        {
            RayGui.GuiSlider(bounds, minBytePtr, maxBytePtr, valuePtr, minValue, maxValue);
        }
    }
}
