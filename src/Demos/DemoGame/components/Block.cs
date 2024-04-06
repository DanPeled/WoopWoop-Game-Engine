using WoopWoop;
using ZeroElectric.Vinculum;

namespace DemoGame
{
    public class Block : Component
    {
        BoxCollider collider;
        public override void Start()
        {
            collider = entity.GetComponent<BoxCollider>();
        }
        public override void Update(float deltaTime)
        {
            if (Ball.Instance is not null)
            {
                if (collider.IsCollidingWith(Ball.Instance.collider))
                {
                    Ball.Instance.velocity *= -1.15f;
                    Ball.Instance.velocity.X *= Math.Sign(new Random().Next(-12, 10));
                    PlayerSlideController.Score += 10;
                    Entity.Destroy(entity);
                }
            }
        }
    }
}