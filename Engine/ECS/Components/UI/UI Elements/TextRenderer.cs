using ZeroElectric.Vinculum;

namespace WoopWoop
{
    namespace UI
    {
        [RequireComponent(typeof(Transform))]
        public class TextRenderer : Renderer
        {
            public string text = "";
            public int fontSize = 15;
            public Font font;
            public float spacing = 1;
            public override void Update(float deltaTime)
            {
                Raylib.DrawTextEx(font, text, transform.Position, fontSize, spacing, Color);
            }
        }
    }
}