using ZeroElectric.Vinculum;
using WoopWoop;
using WoopWoop.UI;
namespace DemoGame
{
    public class DemoGame : Game
    {
        public static Entity player, ball, main, startupText, endScreen;
        List<Entity> blocks = new();

        public override void Start()
        {
            main = new();
            endScreen = new();

            startupText = new();
            startupText.AddComponent<TextRenderer>().text = "PRESS SPACE TO START";
            startupText.GetComponent<TextRenderer>().fontSize = 20;
            startupText.GetComponent<TextRenderer>().Layer = 255;
            startupText.transform.Position = new(WoopWoopEngine.screenWidth / 2 - 20 * 5, WoopWoopEngine.screenHeight / 2);

            player = new(new(WoopWoopEngine.screenWidth / 2, 700));
            player.AddComponent<BoxCollider>();
            player.AddComponent<BasicShapeRenderer>();

            player.AddComponent<PlayerSlideController>();
            player.tag = "player";
            player.transform.Scale = new(70, 10);
            Entity.Instantiate(player);


            ball = new(WoopWoopEngine.screenWidth / 2, 680);
            ball.AddComponent<BasicShapeRenderer>().shape = BasicShape.Ellipse;
            ball.transform.Scale = new(10, 10);
            ball.AddComponent<BoxCollider>();
            Entity.Instantiate(ball);

            Entity text = new();
            player.GetComponent<PlayerSlideController>().textRenderer = text.AddComponent<TextRenderer>();
            Entity.Instantiate(text);

            for (int i = 100; i < 500; i += 40)
            {
                for (int j = 100; j < 900; j += 40)
                {
                    Entity block = new(new(j, i));
                    block.transform.Scale = new(30, 30);
                    block.AddComponent<BoxCollider>();
                    block.AddComponent<BasicShapeRenderer>().Color = Raylib.GOLD;
                    block.AddComponent<Block>();
                    Entity.Instantiate(block);
                    blocks.Add(block);
                    block.Enabled = false;
                }
            }
            Entity.Instantiate(main);


            main.transform.AddChild(ball);
            main.transform.AddChild(player);
            main.transform.AddChild(text);

            endScreen.AddComponent<TextRenderer>().text = "GAME OVER";
            endScreen.transform.Position = new(200, 200);
            endScreen.GetComponent<TextRenderer>().fontSize = 50;
            endScreen.Enabled = false;
            Entity.Instantiate(startupText);
            Entity.Instantiate(endScreen);

        }
        public override void Update()
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                if (ball.GetComponent<Ball>() is null)
                {
                    ball.AddComponent<Ball>();
                    Entity.Destroy(startupText);
                    foreach (var block in blocks)
                    {
                        block.Enabled = true;
                        main.transform.AddChild(block);
                    }
                }
            }
        }
    }
}