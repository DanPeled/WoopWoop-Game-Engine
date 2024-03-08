using Raylib_cs;
using System.Numerics;

namespace WoopWoop
{
    public class ShapeRenderer : Renderer
    {
        public Shape shape;

        public override void Update()
        {
            switch (shape)
            {
                case Shape.Circle:
                    DrawCircle();
                    break;

                case Shape.Rectangle:
                    DrawRectangle();
                    break;
            }
        }

        private void DrawCircle()
        {
            Vector2 position = entity.transform.position;
            float radius = 10 * entity.transform.scale.X;
            Raylib.DrawCircle((int)position.X, (int)position.Y, radius, Color.Black);
        }

        private void DrawRectangle()
        {
            Vector2 position = entity.transform.position;
            Vector2 scale = entity.transform.scale;
            int points = 4; // Number of vertices
            float radius = 10 * scale.X; // Not using rounded corners
            float rotation = entity.transform.Angle + 4543; // No rotation
            Raylib.DrawPoly(position, points, radius, rotation, Color.Black);
        }


        public enum Shape
        {
            Circle,
            Rectangle
        }
    }
}
