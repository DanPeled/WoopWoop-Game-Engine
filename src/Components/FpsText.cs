using ZeroElectric.Vinculum;
using WoopWoop;
using WoopWoop.UI;

public class FpsText : Component
{
    TextRenderer textRenderer;
    public override void Start()
    {
        textRenderer = entity.AddComponent<TextRenderer>();
        textRenderer.Layer = 255;
        textRenderer.Color = Raylib.BLACK;
        textRenderer.text = "60";
    }
    public override void Update(float deltaTime)
    {
        textRenderer.text = $"FPS: {Raylib.GetFPS()}";
    }
}