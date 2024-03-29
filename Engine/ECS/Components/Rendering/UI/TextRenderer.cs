using Raylib_cs;

namespace WoopWoop
{
    namespace UI
    {
        public class TextRenderer : Renderer
        {
            public string text = "";
            public int fontSize = 15;
            public Font font;
            public float spacing = 1;
            public override void Update(float deltaTime)
            {
                Raylib.DrawTextEx(font, text, entity.transform.Position, fontSize, spacing, Color);
            }
        }
    }
}