using ZeroElectric.Vinculum;

namespace WoopWoop.UI
{
    /// <summary>
    /// Represents a text renderer UI element.
    /// </summary>
    [RequireComponent(typeof(Transform))]
    public class TextRenderer : Renderer
    {
        /// <summary>
        /// The text to be rendered.
        /// </summary>
        public string text = "";

        /// <summary>
        /// The font size of the text.
        /// </summary>
        public int fontSize = 15;

        /// <summary>
        /// The font used for rendering the text.
        /// </summary>
        public Font font;

        /// <summary>
        /// The spacing between characters.
        /// </summary>
        public float spacing = 1;

        /// <summary>
        /// Updates the text renderer.
        /// </summary>
        /// <param name="deltaTime">The time since the last frame.</param>
        public override void Update(float deltaTime)
        {
            Raylib.DrawTextEx(font, text, transform.Position, fontSize, spacing, Color);
        }
    }
}
