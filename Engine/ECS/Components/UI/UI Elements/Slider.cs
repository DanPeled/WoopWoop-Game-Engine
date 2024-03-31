using System.Numerics;
using Raylib_cs;

namespace WoopWoop.UI
{
    [RequireComponent(typeof(Transform))]
    public class Slider : Component
    {
        public float min;
        public float max;
        public float intervals;

        public float value; // Current value of the slider

        private bool isDragging; // Flag to track if the knob is being dragged
        public float SliderWidth
        {
            get { return (int)(max - min); }
        }
        public override void Awake()
        {
        }

        public override void Update(float deltaTime)
        {
            // Get the transform component
            Transform transform = entity.GetComponent<Transform>();

            // Calculate the position of the slider's knob based on its value
            float normalizedValue = (value - min) / (max - min);
            float knobX = transform.Position.X + normalizedValue * (max - min) - 5;
            float knobY = transform.Position.Y - 5;

            // Check for mouse input
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                // Get mouse position
                Vector2 mousePos = Raylib.GetMousePosition();

                // Check if mouse is over the knob
                if (Raylib.CheckCollisionPointRec(mousePos, new Raylib_cs.Rectangle(knobX, knobY, 10, 20)))
                {
                    isDragging = true; // Start dragging
                }
            }

            // If mouse button is released, stop dragging
            if (Raylib.IsMouseButtonReleased(MouseButton.Left))
            {
                isDragging = false;
            }

            // If dragging, update value based on mouse position
            if (isDragging)
            {
                Vector2 mousePos = Raylib.GetMousePosition();
                float newValue = min + (mousePos.X - transform.Position.X) * (max - min) / (max - min);
                newValue = Math.Clamp(newValue, min, max);
                value = newValue;
            }

            // Draw the slider
            if (SliderWidth > 0) // Ensure the width is valid
            {
                DrawSliderBody();
                // Draw the slider's knob
                DrawKnob((int)knobX, (int)knobY);
            }
        }
        public virtual void DrawKnob(int knobX, int knobY)
        {
            Raylib.DrawRectangle(knobX, knobY, 10, 20, Color.Red);
        }
        public virtual void DrawSliderBody()
        {
            Raylib.DrawRectangleLines((int)transform.Position.X, (int)transform.Position.Y, (int)SliderWidth, 10, Color.Black);
        }
    }

}
