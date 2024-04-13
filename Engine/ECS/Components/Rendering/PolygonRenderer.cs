using System.Numerics;
using ZeroElectric.Vinculum;

namespace WoopWoop
{
    /// <summary>
    /// Renders a polygon with given vertices.
    /// </summary>
    public class PolygonRenderer : Renderer
    {
        /// <summary>
        /// Vertices of the polygon.
        /// </summary>
        public Vector2[]? vertices; // Vertices of the polygon

        /// <summary>
        /// The thickness of the polygon lines.
        /// </summary>
        public float thickness = 10f;

        /// <summary>
        /// Updates the polygon renderer.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last update.</param>
        public override void Update(float deltaTime)
        {
            if (vertices != null)
            {
                // Rotate the vertices using the entity's transform angle
                Vector2[] rotatedVertices = RotateVertices(vertices, transform.Angle);
                // Draw the rotated polygon
                DrawPolygon((int)transform.Position.X, (int)transform.Position.Y, rotatedVertices, Color);
            }
        }

        // Custom method to draw a polygon
        private void DrawPolygon(int posX, int posY, Vector2[] vertices, Color color)
        {
            if (vertices == null || vertices.Length < 3)
            {
                throw new ArgumentException("Invalid vertices for polygon");
            }

            for (int i = 0; i < vertices.Length - 1; i++)
            {
                Raylib.DrawLineEx(
                    new Vector2((vertices[i].X * transform.Scale.X) + posX, (vertices[i].Y * transform.Scale.Y) + posY),
                    new Vector2((vertices[i + 1].X * transform.Scale.X) + posX, (vertices[i + 1].Y * transform.Scale.Y) + posY),
                    thickness,
                    color
                );
            }

            Raylib.DrawLineEx(
                new Vector2((vertices[vertices.Length - 1].X * transform.Scale.X) + posX, (vertices[vertices.Length - 1].Y * transform.Scale.Y) + posY),
                new Vector2((vertices[0].X * transform.Scale.X) + posX, (vertices[0].Y * transform.Scale.Y) + posY),
                thickness,
                color
            );
        }

        // Custom method to rotate vertices
        private static Vector2[] RotateVertices(Vector2[] vertices, float angle)
        {
            Vector2[] rotatedVertices = new Vector2[vertices.Length];
            Vector2 pivot = Vector2.Zero; // Assuming rotation around the origin for simplicity

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 rotatedPoint = Vector2.Transform(vertices[i] - pivot, Matrix3x2.CreateRotation(angle)) + pivot;
                rotatedVertices[i] = rotatedPoint;
            }

            return rotatedVertices;
        }

        /// <summary>
        /// Contains static properties for basic shapes' vertices.
        /// </summary>
        public static class BasicShapes
        {
            /// <summary>
            /// Vertices for a square.
            /// </summary>
            public static Vector2[] SquareVertices { get; } = new Vector2[]
            {
            new(-0.5f, -0.5f), // Top-left
            new(0.5f, -0.5f),  // Top-right
            new(0.5f, 0.5f),   // Bottom-right
            new(-0.5f, 0.5f)   // Bottom-left
            };

            /// <summary>
            /// Vertices for a triangle.
            /// </summary>
            public static Vector2[] TriangleVertices { get; } = new Vector2[]
            {
            new(0, -50),   // Top
            new(43.3f, 25), // Bottom-right
            new(-43.3f, 25) // Bottom-left
            };
        }
    }
}
