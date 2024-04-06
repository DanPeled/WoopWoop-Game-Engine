using ZeroElectric.Vinculum;

namespace WoopWoop.UI
{
    [RequireComponent(typeof(Transform))]
    public class Canvas : Component
    {
        public Camera camera;
        public override void Awake()
        {
            if (camera == null)
            {
                camera = Camera.Main();
            }
        }

        public override void Update(float deltaTime)
        {
            transform.Position = camera.transform.Position;
        }

        public override void OnDrawGizmo()
        {
            // Calculate the center of the object
            int centerX = (int)(transform.Position.X - transform.Scale.X * 12 / 2);
            int centerY = (int)(transform.Position.Y - transform.Scale.Y * 12 / 2);

            // Calculate the half-width and half-height of the rectangle
            int halfWidth = (int)(transform.Scale.X * 6); // Half of the object's width
            int halfHeight = (int)(transform.Scale.Y * 6); // Half of the object's height

            // Calculate the coordinates for the top-left corner of the rectangle
            int rectX = centerX - halfWidth;
            int rectY = centerY - halfHeight;

            // Draw the rectangle centered on the object
            Raylib.DrawRectangle(rectX, rectY, (int)transform.Scale.X * 30, (int)transform.Scale.Y * 30, new Color(130, 130, 130, 200));
        }
    }
}