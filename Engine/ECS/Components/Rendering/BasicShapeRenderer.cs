using Raylib_cs;
namespace WoopWoop
{
    [RequireComponent(typeof(Transform))]
    public class BasicShapeRenderer : Renderer
    {
        public BasicShape shape;
        public override void Update(float deltaTime)
        {
            switch (shape)
            {
                case BasicShape.Ellipse:
                    {
                        // Calculate the scaled radius for both X and Y axes
                        float scaledRadiusX = transform.Scale.X * 10;
                        float scaledRadiusY = transform.Scale.Y * 10;

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
                        Raylib.DrawRectangle((int)transform.Position.X,
                        (int)transform.Position.Y, (int)transform.Scale.X * 10,
                         (int)transform.Scale.Y * 10, this.Color);
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