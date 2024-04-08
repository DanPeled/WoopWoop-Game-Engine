using ZeroElectric.Vinculum;

namespace WoopWoop.UI
{
    public class Button : UIBoundsComponent
    {

        public string text = "Button";
        public bool state = false;
        public bool prevState = false;
        public bool isToggle = true;
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