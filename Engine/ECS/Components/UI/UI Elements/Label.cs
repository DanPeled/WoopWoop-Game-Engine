using ZeroElectric.Vinculum;

namespace WoopWoop.UI
{
    public class Label : Renderer
    {
        public string text;
        public Rectangle bounds;
        public override void Update(float deltaTime)
        {
            bounds = new Rectangle(transform.Position.X, transform.Position.Y, transform.Scale.X, transform.Scale.Y);

            RayGui.GuiLabel(bounds, " ");
        }
    }
}