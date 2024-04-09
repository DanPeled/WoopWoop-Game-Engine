using ZeroElectric.Vinculum;

namespace WoopWoop.UI
{
    public class Checkbox : UIBoundsComponent
    {
        public string text;
        public bool isChecked;
        public Action<bool> OnIsCheckedChanged;
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