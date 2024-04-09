using ZeroElectric.Vinculum;

namespace WoopWoop.UI
{
    public class DropdownMenu : UIBoundsComponent
    {
        public int activeSelection;
        public bool editMode = true;
        public string itemsString = " ";
        public Action<int> OnSelectionChanged;
        public override void Update(float deltaTime)
        {
            int prev = activeSelection;
            base.Update(deltaTime);
            RayGui.GuiDropdownBox(bounds, itemsString, ref activeSelection, editMode);
            if (activeSelection != prev)
            {
                OnSelectionChanged?.Invoke(activeSelection);
            }
        }
    }
}