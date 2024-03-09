using System.Numerics;

namespace WoopWoop
{
    [RequireComponent(typeof(Transform))]
    public class PhysicsBody : Component
    {
        private Vector2 linearVelocity = Vector2.Zero;
        private float rotationVelocity = 0;

        private float density = 1;
        private float mass = 1;
        private float restitution = 1;
        private float area;

        private bool isStatic = false;

        public override void Update()
        {
            entity.transform.position += linearVelocity;
            linearVelocity += new Vector2(0, 0.01f);
        }
    }
}