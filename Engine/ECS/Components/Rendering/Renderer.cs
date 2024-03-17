using Raylib_cs;
using System;
using System.Numerics;

namespace WoopWoop
{
    [RequireComponent(typeof(Transform))]
    public class Renderer : Component
    {
        public Color Color { get; set; } = Color.Black;
        public byte Layer { get; set; }
    }
    public static class BasicShapes
    {
        // Static property for a square
        public static Vector2[] SquareVertices { get; } = new Vector2[]
        {
            new Vector2(-10, -10), // Top-left
            new Vector2(10, -10),  // Top-right
            new Vector2(10, 10),   // Bottom-right
            new Vector2(-10, 10)   // Bottom-left
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
