using ZeroElectric.Vinculum;
using System;
using System.Numerics;

namespace WoopWoop
{
    [RequireComponent(typeof(Transform))]
    public class Renderer : Component
    {
        public Color Color { get; set; } = Raylib.BLACK;
        private byte layer = 0;
        public byte Layer
        {
            get { return layer; }
            set
            {
                layer = value;
                WoopWoopEngine.ChangeRenderLayer(this);
            }
        }
        protected static Vector2 GetOriginalPositionOfTransform(Transform transform, Pivot pivot)
        {
            Vector2 value = transform.Position;
            switch (pivot)
            {
                case Pivot.Center:
                    {
                        value += transform.Scale / 2;
                        break;
                    }
                case Pivot.BottomRight:
                    {
                        value += transform.Scale;
                        break;
                    }
                case Pivot.TopRight:
                    {
                        value.X += transform.Scale.X;
                        break;
                    }
                case Pivot.BottomLeft:
                    {
                        value.Y += transform.Scale.Y;
                        break;
                    }
                default:
                    break;
            }
            return value;
        }
    }
    public static class BasicShapes
    {
        // Static property for a square
        public static Vector2[] SquareVertices { get; } = new Vector2[]
        {
            new(-10, -10), // Top-left
            new(10, -10),  // Top-right
            new(10, 10),   // Bottom-right
            new(-10, 10)   // Bottom-left
        };

        // Static property for a triangle
        public static Vector2[] TriangleVertices { get; } = new Vector2[]
        {
            new(0, -50),   // Top
            new(43.3f, 25), // Bottom-right
            new(-43.3f, 25) // Bottom-left
        };

    }
}
