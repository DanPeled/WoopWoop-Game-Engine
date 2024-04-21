using WoopWoopEngine;
using System.Numerics;
using ZeroElectric.Vinculum;
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
        transform.Position = new(
            mousePos.X,
             mousePos.Y);
        if (Raylib.IsKeyDown(KeyboardKey.KEY_UP))
        {
            transform.Scale += new Vector2(0, 0.05f);
        }
        else if (Raylib.IsKeyDown(KeyboardKey.KEY_DOWN))
        {
            transform.Scale -= new Vector2(0, 0.05f);
        }
        else if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT))
        {
            transform.Scale -= new Vector2(0.05f, 0);
        }
        else if (Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT))
        {
            transform.Scale += new Vector2(0.05f, 0);
        }
        else if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
        {
            Entity.Destroy(entity);
        }
        transform.Angle += Raylib.GetMouseWheelMove() * 4;
    }
}