using System.Numerics;
using WoopWoop;
using Raylib_cs;
namespace WoopWoop
{
    public class DemoGame : Game
    {
        Entity e = new();
        Vector2 vecA = new(10, 12);
        public override void Start()
        {
            e.transform.position = new Vector2(100, 40);
            Renderer shapeRenderer = e.AddComponent<Renderer>();
            // e.AddComponent<PhysicsBody>();
            shapeRenderer.vertices = BasicShapes.SquareVertices;
            WoopWoopEngine.Instantiate(e);
        }

        public override void Update()
        {
            e.transform.position = Raylib.GetMousePosition();
            if (Raylib.IsKeyDown(KeyboardKey.Up))
            {
                e.transform.scale.Y += 0.5f;
            }
            else if (Raylib.IsKeyDown(KeyboardKey.Down))
            {
                e.transform.scale.Y -= 0.5f;
            }
            else if (Raylib.IsKeyDown(KeyboardKey.Left))
            {
                e.transform.scale.X -= 0.5f;
            }
            else if (Raylib.IsKeyDown(KeyboardKey.Right))
            {
                e.transform.scale.X += 0.5f;
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.Space))
            {
                e.RemoveComponent<Transform>();
            }
            Raylib.DrawText(Raylib.GetFPS().ToString(), 20, 15, 12, Color.Black);
            Raylib.DrawLine(400, 240, 400 - (int)vecA.X, 240 + (int)vecA.Y, Color.Black);
        }
    }
}
