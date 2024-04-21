using System;
using System.Text;
using ZeroElectric.Vinculum;

namespace WoopWoopEngine.UI
{
    /// <summary>
    /// Represents a slider UI element.
    /// </summary>
    public class Slider : UIBoundsComponent
    {
        /// <summary>
        /// The current value of the slider.
        /// </summary>
        public float Value;

        /// <summary>
        /// The text displayed for the minimum value of the slider.
        /// </summary>
        public string MinText = "";

        /// <summary>
        /// The text displayed for the maximum value of the slider.
        /// </summary>
        public string MaxText = "";

        /// <summary>
        /// The minimum value of the slider.
        /// </summary>
        public float MinValue = 0;

        /// <summary>
        /// The maximum value of the slider.
        /// </summary>
        public float MaxValue = 100;

        /// <summary>
        /// Action invoked when the value of the slider changes.
        /// </summary>
        public Action<float> OnValueChange;

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            float prevValue = Value;
            unsafe
            {
                byte[] minBytes = Encoding.ASCII.GetBytes(MinText);
                byte[] maxBytes = Encoding.ASCII.GetBytes(MaxText);

                fixed (byte* minBytePtr = minBytes, maxBytePtr = maxBytes)
                {
                    fixed (float* valuePtr = &Value) // Allocate memory for the float value
                    {
                        Render((sbyte*)minBytePtr, (sbyte*)maxBytePtr, valuePtr);
                    }
                }
            }
            if (Value != prevValue)
            {
                OnValueChange?.Invoke(Value);
            }
        }

        /// <summary>
        /// Renders the slider UI element.
        /// </summary>
        /// <param name="minBytePtr">Pointer to the minimum text.</param>
        /// <param name="maxBytePtr">Pointer to the maximum text.</param>
        /// <param name="valuePtr">Pointer to the value of the slider.</param>
        public unsafe virtual void Render(sbyte* minBytePtr, sbyte* maxBytePtr, float* valuePtr)
        {
            RayGui.GuiSlider(bounds, minBytePtr, maxBytePtr, valuePtr, MinValue, MaxValue);
        }
    }
}
