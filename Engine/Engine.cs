using System;
using System.Collections.Generic;
using System.Diagnostics;
using Raylib_cs;

namespace WoopWoop
{
    public class WoopWoopEngine
    {
        static Game? game;
        private static List<Entity> entities;
        private static readonly object entitiesLock = new object();
        private static Stopwatch stopwatch = new Stopwatch(); // Add a Stopwatch for measuring time

        private static float deltaTime = 0;

        private static void Init(Game game_)
        {
            Raylib.SetTargetFPS(240);
            game = game_;
            entities = new List<Entity>();

            // Start the stopwatch
            stopwatch.Start();
        }

        /// <summary>
        /// Starts up the engine, opens the window and starts everything 
        /// </summary>
        /// <param name="game_">Game object refrence</param>
        public static void Start(Game game_)
        {
            Init(game_);
            Raylib.InitWindow(1089, 720, "Hello World");

            game?.Start();
            while (!Raylib.WindowShouldClose())
            {
                // Measure the deltaTime
                deltaTime = (float)stopwatch.Elapsed.TotalSeconds;
                stopwatch.Restart();

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.White);
                game?.Update();

                // Parallelize entity updates
                entities.ForEach(UpdateEntity);

                Raylib.EndDrawing();
            }
            foreach (Entity entity in entities.ToArray())
            {
                Destroy(entity);
            }
            Raylib.CloseWindow();
        }

        private static void UpdateEntity(Entity e)
        {
            if (e.Enabled)
            {
                e.InternalUpdate(deltaTime);
            }
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

        public static void Destroy(Entity entity)
        {
            var entitiesCopy = new List<Entity>(entities);
            foreach (Transform child in entity.transform.GetChildren())
            {
                StopComponents(child.entity);
                entitiesCopy.Remove(child.entity);
            }
            StopComponents(entity);
            entitiesCopy.Remove(entity);
            entities = entitiesCopy;
        }

        private static void StopComponents(Entity entity)
        {
            foreach (Component c in entity.GetComponents())
            {
                c.Stop();
            }
        }
        public static Entity GetEntityWithUUID(string uuid)
        {
            return entities.FirstOrDefault(e => e.UUID == uuid);
        }
    }
}
