using System;
using ZeroElectric.Vinculum;

namespace WoopWoopEngine.UI
{
    /// <summary>
    /// Represents a checkbox UI element.
    /// </summary>
    public class Checkbox : UIBoundsComponent
    {
        /// <summary>
        /// The text displayed alongside the checkbox.
        /// </summary>
        public string text;

        /// <summary>
        /// Indicates whether the checkbox is checked.
        /// </summary>
        public bool isChecked;

        /// <summary>
        /// Action invoked when the checked state of the checkbox changes.
        /// </summary>
        public Action<bool> OnIsCheckedChanged;

        /// <summary>
        /// Updates the state of the checkbox.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last update.</param>
        public override void Update(float deltaTime)
        {
            bool prev = isChecked;
            base.Update(deltaTime);
            RayGui.GuiCheckBox(bounds, text, ref isChecked);
            if (isChecked != prev)
            {
                OnIsCheckedChanged?.Invoke(isChecked);
            }
        }
    }
}
