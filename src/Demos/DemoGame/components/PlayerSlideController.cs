using WoopWoopEngine;
using WoopWoopEngine.UI;
using ZeroElectric.Vinculum;

namespace DemoGame
{
    public class PlayerSlideController : Component
    {
        public static int Score { get; set; } = 0;
        public TextRenderer textRenderer;
        public override void Update(float deltaTime)
        {
            transform.Position = new(Raylib.GetMousePosition().X, transform.Position.Y);
            if (textRenderer != null)
            {
                textRenderer.text = $"{Score} Pts";
            }
        }
    }
}