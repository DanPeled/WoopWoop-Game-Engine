using System.Numerics;
using ZeroElectric.Vinculum;
using WoopWoopEngine;

namespace DemoGame
{
    public class Ball : Component
    {
        public BoxCollider2D collider;
        public static Ball Instance { get; private set; }
        public Vector2 velocity = Vector2.One;
        public override void Awake()
        {
            Instance = this;
            collider = entity.GetComponent<BoxCollider2D>();
            entity.tag = "ball";
        }
        public override void Update(float deltaTime)
        {
            Instance = this;
            collider ??= entity.GetComponent<BoxCollider2D>();

            if (collider.GetCollidingEntitiesWithTag("player").Length > 0)
            {
                Random random = new Random();
                velocity = new((velocity.X + random.Next(0, 10) / 10f) * (Raylib.GetMouseDelta().X != 0 ? Math.Sign(Raylib.GetMouseDelta().X) : 1), velocity.Y * -1);
                CollisionData? collisionData = collider.GetCollisionData(Entity.GetEntitiesWithTag("player")[0].GetComponent<BoxCollider2D>());
            }
            if (IsCollidingWithScreenBoundsY())
            {
                if (transform.Position.Y >= WoopWoop.screenHeight)
                {
                    DemoGame.endScreen.Enabled = true;
                    DemoGame.main.Enabled = false;
                }

                velocity = new(velocity.X * 0.98f, velocity.Y * -0.98f);
            }
            else if (IsCollidingWithScreenBoundsX())
            {
                velocity = new Vector2(velocity.X * -0.98f, velocity.Y * 0.98f);
            }

            transform.Position += velocity;
            velocity *= 1.0001f;

            velocity.X = Math.Clamp(velocity.X, -15, 15);
            velocity.Y = Math.Clamp(velocity.Y, -15, 15);
        }
        public bool IsCollidingWithScreenBoundsX()
        {
            return transform.Position.X <= 0 || transform.Position.X >= WoopWoop.screenWidth - transform.Scale.X;
        }
        public bool IsCollidingWithScreenBoundsY()
        {
            return transform.Position.Y <= 0 || transform.Position.Y >= WoopWoop.screenHeight;
        }
    }
}