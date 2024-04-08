using ZeroElectric.Vinculum;

namespace WoopWoop.UI
{
    public class UIWindow : Renderer
    {
        public Rectangle bounds;
        public override void Update(float deltaTime)
        {
            bounds = new Rectangle(transform.Position.X, transform.Position.Y, transform.Scale.X, transform.Scale.Y);

            RayGui.GuiDummyRec(bounds, " ");
        }
    }
}