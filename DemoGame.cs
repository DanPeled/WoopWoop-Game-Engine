using System.Numerics;
using WoopWoop;
using Raylib_cs;
namespace WoopWoop
{
    public class DemoGame : Game
    {
        Entity e = new();
        public override void Start()
        {
            e.transform.position = new Vector2(100, 40);
            ShapeRenderer shapeRenderer = e.AddComponent<ShapeRenderer>();
            shapeRenderer.shape = ShapeRenderer.Shape.Rectangle;
            WoopWoopEngine.Instantiate(e);
        }

        public override void Update()
        {
            e.transform.position = Raylib.GetMousePosition();
            if (Raylib.IsKeyDown(KeyboardKey.Space))
            {
                e.transform.Angle += 1;
            }
            Console.WriteLine(e.transform.Angle);
        }
    }
}
