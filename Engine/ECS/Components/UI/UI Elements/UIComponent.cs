using ZeroElectric.Vinculum;

namespace WoopWoop.UI
{
    /// <summary>
    /// Represents a UI component with defined bounds.
    /// </summary>
    public class UIBoundsComponent : Renderer
    {
        /// <summary>
        /// The bounds of the UI component.
        /// </summary>
        public Rectangle bounds;

        /// <summary>
        /// Updates the UI component.
        /// </summary>
        /// <param name="deltaTime">The time since the last frame.</param>
        public override void Update(float deltaTime)
        {
            bounds = new Rectangle(transform.Position.X, transform.Position.Y, transform.Scale.X, transform.Scale.Y);
        }
    }
}
