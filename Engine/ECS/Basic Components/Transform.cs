using System.Numerics;

namespace WoopWoop
{
    public class Transform : Component
    {
        public Vector2 position = Vector2.Zero;
        public Vector2 scale = Vector2.One;
        private float angle = 0;
        public float Angle
        {
            get
            {
                return angle;
            }
            set
            {
                angle = value % 360;
            }
        }
        public override void Start()
        {
        }
        public override void Update()
        {
        }
    }
}