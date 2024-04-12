using ZeroElectric.Vinculum;

namespace WoopWoop.UI
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