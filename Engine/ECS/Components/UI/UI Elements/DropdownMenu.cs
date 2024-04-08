using ZeroElectric.Vinculum;

namespace WoopWoop.UI
{
    public class DropdownMenu : UIBoundsComponent
    {
        public int activeSelection;
        public bool editMode = true;
        public string itemsString = " ";

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            RayGui.GuiDropdownBox(bounds, itemsString, ref activeSelection, editMode);
        }
    }
}