using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Raylib_cs;

namespace WoopWoop
{
    public class WoopWoopEngine
    {
        static Game? game;
        private static List<Entity> entities;
        private static readonly object entitiesLock = new object();

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

                // Parallelize entity updates
                Parallel.ForEach(entities.ToArray(), e =>
                {
                    UpdateEntity(e);
                });

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }

        private static void UpdateEntity(Entity e)
        {
            e.Update();
            e.InternalUpdate();
        }

        public static void Instantiate(Entity entity)
        {
            lock (entitiesLock)
            {
                entities.Add(entity);
            }
            foreach (var component in entity.GetComponents())
            {
                component.Start();
            }
        }
    }

}
