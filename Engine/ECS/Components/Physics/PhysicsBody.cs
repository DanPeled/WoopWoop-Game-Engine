using System.Numerics;

namespace WoopWoop
{
    [RequireComponent(typeof(Transform))]
    public class PhysicsBody : Component
    {
        private Vector2 linearVelocity = Vector2.Zero;
        private float rotationVelocity = 0;

        public float density = 1;
        public float mass = 1;
        public float restitution = 1;
        public float area;

        public bool isStatic = false;

        public float radius, width, height;
        public BasicShape shape;

        public override void Update(float deltaTime)
        {
            entity.transform.Position += linearVelocity;
            linearVelocity += new Vector2(0, 0.01f);
        }
    }
}