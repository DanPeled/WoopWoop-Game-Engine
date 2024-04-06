using System.Numerics;
using ZeroElectric.Vinculum;

namespace WoopWoop
{
    public class PolygonRenderer : Renderer
    {
        public Vector2[]? vertices; // Vertices of the polygon

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

            // Draw lines between consecutive vertices to form the polygon
            for (int i = 0; i < vertices.Length - 1; i++)
            {
                Raylib.DrawLine(
                    (int)(vertices[i].X * transform.Scale.X) + posX,
                    (int)(vertices[i].Y * transform.Scale.Y) + posY,
                    (int)(vertices[i + 1].X * transform.Scale.X) + posX,
                    (int)(vertices[i + 1].Y * transform.Scale.Y) + posY,
                    color
                );
            }

            // Draw a line connecting the last vertex to the first to close the polygon
            Raylib.DrawLine(
                (int)(vertices[vertices.Length - 1].X * transform.Scale.X) + posX,
                (int)(vertices[vertices.Length - 1].Y * transform.Scale.Y) + posY,
                (int)(vertices[0].X * transform.Scale.X) + posX,
                (int)(vertices[0].Y * transform.Scale.Y) + posY,
                color
            );
        }


        // Custom method to rotate vertices
        private Vector2[] RotateVertices(Vector2[] vertices, float angle)
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
    }
}