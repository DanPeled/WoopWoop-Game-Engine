using System.Numerics;
using WoopWoop;
using Raylib_cs;
using WoopWoop.UI;

public class DemoGame : Game
{
    Vector2 vecA = new(10, 12);
    public override void Start()
    {
        Entity e = new();
        e.transform.Position = new Vector2(100, 40);
        TextureRenderer textureRenderer = e.AddComponent<TextureRenderer>();
        e.AddComponent<PlayerController>();
        Image image = Raylib.LoadImage("resources/r.png");
        Raylib.ImageResize(ref image, 100, 100);
        textureRenderer.LoadImage(image);
        textureRenderer.Color = Color.White;
        WoopWoopEngine.Instantiate(e);

        Entity e2 = new();
        e2.transform.Position = new Vector2(100, 100);
        BasicShapeRenderer shapeRenderer2 = e2.AddComponent<BasicShapeRenderer>();
        shapeRenderer2.shape = BasicShape.Ellipse;
        WoopWoopEngine.Instantiate(e2);

        e.transform.AddChild(e2);

        Entity fpsText = new();
        fpsText.AddComponent<FpsText>();
        WoopWoopEngine.Instantiate(fpsText);

        Entity canvas = new();
        canvas.AddComponent<Canvas>();
        WoopWoopEngine.Instantiate(canvas);
    }
}
