using ZeroElectric.Vinculum;

namespace WoopWoopEngine.UI
{
    /// <summary>
    /// Represents a canvas UI element.
    /// </summary>
    [RequireComponent(typeof(Transform))]
    public class Canvas : Component
    {
        /// <summary>
        /// The camera associated with the canvas.
        /// </summary>
        public Camera camera;

        /// <summary>
        /// Initializes the canvas by assigning a default camera if none is provided.
        /// </summary>
        public override void Awake()
        {
            if (camera == null)
            {
                camera = Camera.Main();
            }
        }

        /// <summary>
        /// Updates the canvas position to match the camera's position.
        /// </summary>
        /// <param name="deltaTime">The time since the last frame.</param>
        public override void Update(float deltaTime)
        {
            transform.Position = camera.transform.Position;
        }

        /// <summary>
        /// Draws the canvas boundaries as a rectangle in the editor.
        /// </summary>
        public override void OnDrawGizmo()
        {
            // Calculate the center of the object
            int centerX = (int)(transform.Position.X - transform.Scale.X * 12 / 2);
            int centerY = (int)(transform.Position.Y - transform.Scale.Y * 12 / 2);

            // Calculate the half-width and half-height of the rectangle
            int halfWidth = (int)(transform.Scale.X * 6); // Half of the object's width
            int halfHeight = (int)(transform.Scale.Y * 6); // Half of the object's height

            // Calculate the coordinates for the top-left corner of the rectangle
            int rectX = centerX - halfWidth;
            int rectY = centerY - halfHeight;

            // Draw the rectangle centered on the object
            Raylib.DrawRectangle(rectX, rectY, (int)transform.Scale.X * 30, (int)transform.Scale.Y * 30, new Color(130, 130, 130, 200));
        }
    }
}
