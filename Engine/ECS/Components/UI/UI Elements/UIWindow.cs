using ZeroElectric.Vinculum;

namespace WoopWoop.UI
{
    /// <summary>
    /// Represents a window UI element.
    /// </summary>
    public class UIWindow : UIBoundsComponent
    {
        /// <summary>
        /// Updates the window UI element.
        /// </summary>
        /// <param name="deltaTime">The time since the last frame.</param>
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            // Draw an invisible rectangle to define the window area
            RayGui.GuiDummyRec(bounds, " ");
        }
    }
}
