using ZeroElectric.Vinculum;

namespace WoopWoopEngine.UI
{
    public class Label : UIBoundsComponent
    {
        public string text;
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            RayGui.GuiLabel(bounds, " ");
        }
    }
}