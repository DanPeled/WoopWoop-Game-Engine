using ZeroElectric.Vinculum;
using System.Numerics;

namespace WoopWoop
{
    /// <summary>
    /// A renderer for basic shapes such as ellipses and rectangles.
    /// </summary>
    [RequireComponent(typeof(Transform))]
    public class BasicShapeRenderer : Renderer
    {
        /// <summary>
        /// The shape to render.
        /// </summary>
        public BasicShape shape = BasicShape.Box;

        /// <summary>
        /// Updates the basic shape renderer.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last update.</param>
        public override void Update(float deltaTime)
        {
            switch (shape)
            {
                case BasicShape.Ellipse:
                    {
                        // Draw an ellipse with scaled radius
                        Raylib.DrawEllipse(
                            (int)transform.Position.X,
                            (int)transform.Position.Y,
                            (int)transform.Scale.X, // Scale X as width
                            (int)transform.Scale.Y, // Scale Y as height
                            Color);
                        break;
                    }
                case BasicShape.Box:
                    {
                        // Calculate the position of the rectangle considering rotation
                        Vector2 position = new Vector2(transform.Position.X, transform.Position.Y);
                        Vector2 origin = new Vector2(transform.Scale.X / 4, transform.Scale.Y / 4);
                        Matrix3x2 transformMatrix = Matrix3x2.CreateRotation(transform.Angle * (float)Math.PI / 180f, origin) *
                            Matrix3x2.CreateTranslation(position - origin);

                        // Apply transformation to the rectangle's position
                        Vector2 transformedPosition = Vector2.Transform(Vector2.Zero, transformMatrix);

                        // Draw a rectangle with scaled width, height, and rotation
                        Raylib.DrawRectanglePro(
                            new Rectangle(transformedPosition.X, transformedPosition.Y, transform.Scale.X, transform.Scale.Y),
                            origin,
                            transform.Angle, // Rotation angle in degrees (change this as needed)
                            Color);
                        break;
                    }
            }
        }
    }

    /// <summary>
    /// Represents basic shapes that can be rendered.
    /// </summary>
    public enum BasicShape
    {
        /// <summary>
        /// Ellipse shape.
        /// </summary>
        Ellipse = 0,

        /// <summary>
        /// Rectangle shape.
        /// </summary>
        Box = 1
    }
}
