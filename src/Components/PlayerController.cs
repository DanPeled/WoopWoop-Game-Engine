using WoopWoop;
using System.Numerics;
using Raylib_cs;
public class PlayerController : Component
{
    TextureRenderer textureRenderer;
    public override void Start()
    {
        textureRenderer = entity.GetComponent<TextureRenderer>();
        // entity.AddComponent<ImageSpammer>();
        // entity.RemoveComponent<TextureRenderer>();
        // entity.AddComponent<BasicShapeRenderer>().shape = BasicShape.Box;
    }
    public override void Update(float deltaTime)
    {
        Vector2 mousePos = Raylib.GetMousePosition();
        entity.transform.Position = new(
            mousePos.X,
             mousePos.Y);
        if (Raylib.IsKeyDown(KeyboardKey.Up))
        {
            entity.transform.Scale += new Vector2(0, 0.05f);
        }
        else if (Raylib.IsKeyDown(KeyboardKey.Down))
        {
            entity.transform.Scale -= new Vector2(0, 0.05f);
        }
        else if (Raylib.IsKeyDown(KeyboardKey.Left))
        {
            entity.transform.Scale -= new Vector2(0.05f, 0);
        }
        else if (Raylib.IsKeyDown(KeyboardKey.Right))
        {
            entity.transform.Scale += new Vector2(0.05f, 0);
        }
        else if (Raylib.IsKeyPressed(KeyboardKey.Space))
        {
            WoopWoopEngine.Destroy(entity);
        }
        else if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            entity.Enabled = !entity.Enabled;
        }
    }
}