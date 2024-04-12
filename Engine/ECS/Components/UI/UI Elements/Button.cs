using ZeroElectric.Vinculum;

namespace WoopWoop.UI
{
    /// <summary>
    /// Represents a button UI element.
    /// </summary>
    public class Button : UIBoundsComponent
    {
        /// <summary>
        /// The text displayed on the button.
        /// </summary>
        public string text = "Button";

        /// <summary>
        /// Indicates whether the button is currently pressed.
        /// </summary>
        public bool state = false;

        /// <summary>
        /// Indicates the previous state of the button.
        /// </summary>
        public bool prevState = false;

        /// <summary>
        /// Indicates whether the button behaves as a toggle button.
        /// </summary>
        public bool isToggle = true;

        /// <summary>
        /// Action invoked when the button is pressed.
        /// </summary>
        public Action OnPressed;

        public override void Update(float deltaTime)
        {
            prevState = state;
            base.Update(deltaTime);

            bool pressed = RayGui.GuiButton(bounds, text) == 1;

            if (pressed)
            {
                OnPressed?.Invoke();
            }

            if (isToggle)
            {
                if (pressed)
                {
                    state = !state;
                }
            }
            else
            {
                state = pressed;
            }
        }
    }
}
