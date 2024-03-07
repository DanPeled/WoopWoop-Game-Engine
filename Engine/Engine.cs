using Raylib_cs;

namespace WoopWoop
{
    public class WoopWoopEngine
    {
        static Game? game;
        private static List<Entity> entities;
        private static void Init(Game game_)
        {
            game = game_;
            entities = new List<Entity>();
        }
        public static void Start(Game game_)
        {
            Init(game_);
            Raylib.InitWindow(800, 480, "Hello World");

            game?.Start();
            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.White);

                foreach (Entity e in entities.ToArray())
                {
                    e.Update();
                    e.InternalUpdate();
                }

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }
        public static void Instantiate(Entity entity)
        {
            entities.Add(entity);
            foreach (var component in entity.GetComponents())
            {
                component.Start();
            }
        }
    }
}