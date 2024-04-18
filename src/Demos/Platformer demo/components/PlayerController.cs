using System;
using System.Collections.Generic;
using System.Numerics;
using WoopWoop;
using ZeroElectric.Vinculum;
using System.Linq;

namespace PlatformerDemo
{
    public class PlayerController : Component
    {
        private Vector2 velocity = new Vector2(0, 100);
        private Vector2 acceleration = new Vector2(0, 0);
        private float accelerationRate = 0.1f;
        private float maxSpeed = 40000f;
        private float gravity = 0.5f;
        private BoxCollider rightCollider, leftCollider;
        private bool collidingOnLeft, collidingOnRight, isCollidingOnTop;
        private CollisionData? collisionData;
        public override void Awake()
        {
            Entity rightCollider = Entity.CreateEntity()
            // .AddComponent<BasicShapeRenderer>()
            .AddComponent<BoxCollider>().SetPosition(transform.Position + new Vector2(transform.Scale.X - 10, 1)).SetScale(new Vector2(15, transform.Scale.Y - 4)).Create();
            Entity.Instantiate(rightCollider);
            transform.AddChild(rightCollider);

            Entity leftCollider = Entity.CreateEntity()
            // .AddComponent<BasicShapeRenderer>()
            .AddComponent<BoxCollider>().SetPosition(transform.Position - new Vector2(transform.Scale.X - 10, 1)).SetScale(new Vector2(15, transform.Scale.Y - 4)).Create();
            Entity.Instantiate(leftCollider);
            transform.AddChild(leftCollider);

            this.rightCollider = rightCollider.GetComponent<BoxCollider>();
            this.leftCollider = leftCollider.GetComponent<BoxCollider>();
        }
        public override void Update(float deltaTime)
        {
            HandleInput(deltaTime);
            IsGrounded();
            // Apply gravity
            velocity.Y += gravity;

            // Update velocity based on acceleration
            velocity += acceleration;
            // Limit velocity to max speed
            velocity.X = Math.Clamp(velocity.X, -maxSpeed, maxSpeed);
            velocity.Y = Math.Clamp(velocity.Y, -maxSpeed, maxSpeed);
            transform.Position = new Vector2(Math.Clamp(transform.Position.X, transform.Scale.X, 800 - transform.Scale.X), transform.Position.Y);
            // Update X and Y separately in the loop
            for (int i = 0; i < Math.Abs(velocity.X); i++)
            {
                UpdatePosition(new Vector2(Math.Sign(velocity.X), 0));
            }

            for (int j = 0; j < Math.Abs(velocity.Y); j++)
            {
                UpdatePosition(new Vector2(0, Math.Sign(velocity.Y)));
            }

            // Apply friction
            velocity *= 0.99f;
            Raylib.DrawFPS(10, 10);
        }

        private void UpdatePosition(Vector2 delta)
        {
            IsGrounded();
            if (collidingOnLeft && delta.X <= 0)
            {
                acceleration.X = 0;
                velocity.X = 0;
                delta.X = 0;
            }
            else if (collidingOnRight && delta.X >= 0)
            {
                velocity.X = 0;
                delta.X = 0;
            }

            else if (isCollidingOnTop)
            {
                velocity.Y = gravity;
                velocity.Y += acceleration.Y;
            }
            if (IsGrounded() && velocity.Y >= 0)
            {
                velocity.Y = 0; // Stop vertical movement if grounded
                delta.Y = 0;
            }

            transform.Position += delta;
        }

        private void HandleInput(float deltaTime)
        {
            float jumpForce = 10f;
            // Reset acceleration
            acceleration = Vector2.Zero;

            // Check for movement input
            if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT))
            {
                acceleration.X = -accelerationRate; // Apply leftward acceleration
            }
            else if (Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT))
            {
                acceleration.X = accelerationRate; // Apply rightward acceleration
            }

            // Apply deceleration if no movement input
            if (!Raylib.IsKeyDown(KeyboardKey.KEY_LEFT) && !Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT))
            {
                // Apply deceleration in the opposite direction of current velocity
                acceleration.X = -Math.Sign(velocity.X) * accelerationRate;
            }

            // Handle jumping
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE) && IsGrounded())
            {
                Jump(jumpForce);
            }
        }
        public void Jump(float height)
        {
            acceleration.Y = -height; // Set upward velocity for jump
        }
        private void HandleCollision(CollisionData? data)
        {
            if (data != null)
            {
                switch (data.Value.entity.tag)
                {
                    default:
                        {
                            break;
                        }
                    case "jumpPad":
                        {
                            Jump(13f);
                            break;
                        }
                }
            }
        }

        public bool IsGrounded()
        {
            List<Entity> entities = Entity.GetEntitiesWithComponent<BoxCollider>().ToList();
            entities.Remove(entity);

            foreach (BoxCollider other in entities.ConvertAll(e => e.GetComponent<BoxCollider>()))
            {
                if (other.entity.tag == "wall")
                {
                    collidingOnRight = rightCollider.IsCollidingWith(other);
                    collidingOnLeft = leftCollider.IsCollidingWith(other);
                    if (entity.GetComponent<BoxCollider>().IsCollidingWith(other))
                    {
                        collisionData = (CollisionData)entity.GetComponent<BoxCollider>().GetCollisionData(other);
                        // isCollidingOnTop = collisionData.Value.contactPoints.ToList().Any((p) =>
                        // {
                        //     return transform.Position - p == new Vector2(15, 15);
                        // });
                        isCollidingOnTop = false;
                        HandleCollision(collisionData);
                        return !isCollidingOnTop;
                    }
                }
            }

            return false;
        }
    }
}
