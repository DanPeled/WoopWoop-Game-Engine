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
                case BasicShape.Circle:
                    {
                        Raylib.DrawCircle((int)entity.transform.Position.X,
                         (int)entity.transform.Position.Y,
                          entity.transform.Scale.X * 10, this.color);
                        break;
                    }
                case BasicShape.Box:
                    {
                        Raylib.DrawRectangle((int)entity.transform.Position.X,
                        (int)entity.transform.Position.Y, (int)entity.transform.Scale.X * 10,
                         (int)entity.transform.Scale.Y * 10, this.color);
                        break;
                    }
            }
        }
    }

    public enum BasicShape
    {
        Circle = 0,
        Box = 1
    }
}