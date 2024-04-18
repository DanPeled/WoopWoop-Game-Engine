using WoopWoop;
using ZeroElectric.Vinculum;

namespace PlatformerDemo
{
    public class PlatformerDemo : Game
    {
        public override void Start()
        {
            Entity player = Entity.CreateEntity()
            .AddComponent<BoxCollider>()
            .AddComponent<BasicShapeRenderer>()
            .AddComponent<PlayerController>()
            .SetScale(30, 30)
            .SetPosition(new(400, 400))
            .Create();
            player.GetComponent<BasicShapeRenderer>().Color = Raylib.GRAY;

            Entity floor = Entity.CreateEntity()
            .Tag("wall")
            .AddComponent<BoxCollider>()
            .AddComponent<BasicShapeRenderer>()
            .SetPosition(new(0, 1200))
            .SetScale(3000, 1000)
            .Create();

            for (int i = 0; i < 13; i++)
            {
                Entity wall = Entity.CreateEntity()
                .Tag("wall").AddComponent<BoxCollider>()
                .AddComponent<BasicShapeRenderer>()
                .SetPosition(new(400 + i * 30, 700 + i * -10)).SetScale(30, 90 + (i * 15)).Create();
                Entity.Instantiate(wall);
            }


            Entity jumpPad = Entity.CreateEntity()
            .AddComponent<BoxCollider>()
            .Tag("jumpPad")
            .AddComponent<BasicShapeRenderer>()
            .SetPosition(new(900, 700))
            .SetScale(30, 30).Create();
            jumpPad.GetComponent<BasicShapeRenderer>().Color = Raylib.RED;

            // Entity.Instantiate(Entity.CreateEntity().AddComponent<BasicShapeRenderer>().SetPosition(new(0, 0)).SetScale(12, 10000).AddComponent<BoxCollider>().Create());
            Entity.Instantiate(jumpPad);
            Entity.Instantiate(player);
            Entity.Instantiate(floor);
        }
    }
}