using Raylib_cs;
using System;
using System.Numerics;

namespace WoopWoop
{
    public class Renderer : Component
    {
        public Vector2[]? vertices; // Vertices of the polygon
        public Texture2D? texture; // Texture to be drawn


        public override void Update()
        {
            if (vertices != null)
            {
                // Rotate the vertices using the entity's transform angle
                Vector2[] rotatedVertices = RotateVertices(vertices, entity.transform.Angle);
                // Draw the rotated polygon
                DrawPolygon((int)entity.transform.position.X, (int)entity.transform.position.Y, rotatedVertices, Color.Black);
            }
            else if (texture != null)
            {
                // Draw the texture
                Raylib.DrawTexture((Texture2D)texture, (int)entity.transform.position.X, (int)entity.transform.position.Y, Color.Black);
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
                Raylib.DrawLine((int)vertices[i].X + posX, (int)vertices[i].Y + posY, (int)vertices[i + 1].X + posX, (int)vertices[i + 1].Y + posY, color);
            }

            // Draw a line connecting the last vertex to the first to close the polygon
            Raylib.DrawLine((int)vertices[vertices.Length - 1].X + posX, (int)vertices[vertices.Length - 1].Y + posY, (int)vertices[0].X + posX, (int)vertices[0].Y + posY, color);
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
    public static class BasicShapes
    {
        // Static property for a square
        public static Vector2[] SquareVertices { get; } = new Vector2[]
        {
            new Vector2(-50, -50), // Top-left
            new Vector2(50, -50),  // Top-right
            new Vector2(50, 50),   // Bottom-right
            new Vector2(-50, 50)   // Bottom-left
        };

        // Static property for a triangle
        public static Vector2[] TriangleVertices { get; } = new Vector2[]
        {
            new Vector2(0, -50),   // Top
            new Vector2(43.3f, 25), // Bottom-right
            new Vector2(-43.3f, 25) // Bottom-left
        };

    }
}
