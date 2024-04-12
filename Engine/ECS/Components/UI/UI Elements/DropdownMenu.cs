using ZeroElectric.Vinculum;

namespace WoopWoop.UI
{
    /// <summary>
    /// Represents a dropdown menu UI element.
    /// </summary>
    public class DropdownMenu : UIBoundsComponent
    {
        /// <summary>
        /// The index of the currently active selection.
        /// </summary>
        public int activeSelection;

        /// <summary>
        /// Indicates whether the dropdown menu is in edit mode.
        /// </summary>
        public bool editMode = true;

        /// <summary>
        /// The string containing items for the dropdown menu.
        /// </summary>
        public string itemsString = " ";

        /// <summary>
        /// Action invoked when the selection in the dropdown menu changes.
        /// </summary>
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
