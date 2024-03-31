using Raylib_cs;
using System;
using System.Numerics;

namespace WoopWoop
{
    [RequireComponent(typeof(Transform))]
    public class BasicShapeRenderer : Renderer
    {
        public BasicShape shape = BasicShape.Box;
        public override void Update(float deltaTime)
        {
            switch (shape)
            {
                case BasicShape.Ellipse:
                    {
                        // Calculate the scaled radius for both X and Y axes
                        float scaledRadiusX = transform.Scale.X * 5;
                        float scaledRadiusY = transform.Scale.Y * 5;

                        // Draw an ellipse with scaled radius
                        Raylib.DrawEllipse(
                            (int)transform.Position.X,
                            (int)transform.Position.Y,
                            scaledRadiusX,
                            scaledRadiusY,
                            this.Color);
                        break;
                    }
                case BasicShape.Box:
                    {
                        Raylib.DrawRectanglePro(
                             new Rectangle(
                                transform.Position.X + 5f * transform.Scale.X,
                                transform.Position.Y + 5f * transform.Scale.Y,
                                transform.Scale.X * 10,
                                transform.Scale.Y * 10
                        ),
                        new Vector2(
                            transform.Scale.X * 5,  // origin x
                            transform.Scale.Y * 5   // origin y
                        ),
                        transform.Angle,         // rotation
                        this.Color
                        );

                        break;
                    }
            }
        }
    }

    public enum BasicShape
    {
        Ellipse = 0,
        Box = 1
    }
}
