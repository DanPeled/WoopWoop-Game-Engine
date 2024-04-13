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
        public int ActiveSelection;

        /// <summary>
        /// Indicates whether the dropdown menu is in edit mode.
        /// </summary>
        public bool EditMode = true;

        /// <summary>
        /// The string containing items for the dropdown menu.
        /// </summary>
        public string ItemsString = " ";

        /// <summary>
        /// Action invoked when the selection in the dropdown menu changes.
        /// </summary>
        public Action<int> OnSelectionChanged;

        public override void Update(float deltaTime)
        {
            int prev = ActiveSelection;
            base.Update(deltaTime);
            RayGui.GuiDropdownBox(bounds, ItemsString, ref ActiveSelection, EditMode);
            if (ActiveSelection != prev)
            {
                OnSelectionChanged?.Invoke(ActiveSelection);
            }
        }
    }
}
