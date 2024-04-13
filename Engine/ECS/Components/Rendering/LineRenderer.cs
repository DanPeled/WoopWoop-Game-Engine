using System.Numerics;
using ZeroElectric.Vinculum;

namespace WoopWoop
{
    /// <summary>
    /// Renders a line between two points.
    /// </summary>
    public class LineRenderer : Renderer
    {
        /// <summary>
        /// The end position of the line.
        /// </summary>
        public Vector2 endPosition;

        /// <summary>
        /// Determines if the end position is relative to the start position.
        /// </summary>
        public bool isEndPositionRelative = true;

        /// <summary>
        /// The thickness of the line.
        /// </summary>
        public float thickness = 3;

        /// <summary>
        /// Updates the line renderer.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last update.</param>
        public override void Update(float deltaTime)
        {
            Raylib.DrawLineEx(transform.Position, isEndPositionRelative ? transform.Position - endPosition : endPosition, thickness, Color);
        }
    }
}
