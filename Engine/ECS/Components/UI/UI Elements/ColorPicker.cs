using ZeroElectric.Vinculum;

namespace WoopWoop.UI
{
    /// <summary>
    /// Represents a color picker UI element.
    /// </summary>
    public class ColorPicker : UIBoundsComponent
    {
        /// <summary>
        /// Updates the color picker.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last update.</param>
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            RayGui.GuiColorPicker(bounds, "", ref Color);
        }
    }
}
