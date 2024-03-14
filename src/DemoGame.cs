using System.Numerics;
using WoopWoop;
using Raylib_cs;
namespace WoopWoop
{
    public class DemoGame : Game
    {
        Vector2 vecA = new(10, 12);
        public override void Start()
        {

            Entity e = new();
            e.transform.Position = new Vector2(100, 40);
            TextureRenderer shapeRenderer = e.AddComponent<TextureRenderer>();
            e.AddComponent<PlayerController>();
            // e.AddComponent<PhysicsBody>();
            shapeRenderer.LoadFromPath("resources/r.png");
            shapeRenderer.color = Color.White;
            WoopWoopEngine.Instantiate(e);

            Entity e2 = new();
            e2.transform.Position = new Vector2(100, 100);
            BasicShapeRenderer shapeRenderer2 = e2.AddComponent<BasicShapeRenderer>();
            // e.AddComponent<PhysicsBody>();
            shapeRenderer2.shape = BasicShape.Circle;
            WoopWoopEngine.Instantiate(e2);
            e.AddChild(e2);
        }

        public override void Update()
        {
            Raylib.DrawText(Raylib.GetFPS().ToString(), 20, 15, 12, Color.Black);
        }
    }
}
