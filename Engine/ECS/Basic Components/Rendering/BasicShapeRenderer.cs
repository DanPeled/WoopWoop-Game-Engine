using Raylib_cs;
namespace WoopWoop
{
    [RequireComponent(typeof(Transform))]
    public class BasicShapeRenderer : Renderer
    {
        public BasicShape shape;
        public override void Update()
        {
            switch (shape)
            {
                case BasicShape.Circle:
                    {
                        Raylib.DrawCircle((int)entity.transform.position.X,
                         (int)entity.transform.position.Y,
                          entity.transform.scale.X * 10, Color.Black);
                        break;
                    }
                case BasicShape.Box:
                    {
                        Raylib.DrawRectangle((int)entity.transform.position.X,
                        (int)entity.transform.position.Y, (int)entity.transform.scale.X * 10,
                         (int)entity.transform.scale.Y * 10, Color.Black);
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