using System.Numerics;

namespace WoopWoop
{
    public class Transform : Component
    {
        private Vector2 position = Vector2.Zero;
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        private Vector2 scale = Vector2.One;
        public Vector2 Scale
        {
            get { return scale; }
            set
            {
                foreach (Transform childTransform in entity.GetChildren().ToList().Select(e => e.transform))
                {
                    childTransform.Scale = value;
                }
                scale = value;
            }
        }
        private float angle = 0;
        public float Angle
        {
            get
            {
                return angle;
            }
            set
            {
                angle = (value % 360 + 360) % 360;
            }
        }
    }
}