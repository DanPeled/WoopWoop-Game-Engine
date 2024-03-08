using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Raylib_cs;

namespace WoopWoop
{
    /// <summary>
    /// Represents the main engine for the WoopWoop game.
    /// </summary>
    public class WoopWoopEngine
    {
        static Game? game; // The game instance
        private static List<Entity> entities; // List of entities in the game
        private static readonly object entitiesLock = new object(); // Lock object for synchronizing access to the entities list

        /// <summary>
        /// Initializes the game engine.
        /// </summary>
        /// <param name="game_">The Game instance to use.</param>
        private static void Init(Game game_)
        {
            Raylib.SetTargetFPS(60);
            game = game_;
            entities = new List<Entity>();
        }

        /// <summary>
        /// Starts the game engine.
        /// </summary>
        /// <param name="game_">The Game instance to use.</param>
        public static void Start(Game game_)
        {
            Init(game_);
            Raylib.InitWindow(800, 480, "Hello World");

            game?.Start();
            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.White);
                game?.Update();
                // Parallelize entity updates
                Parallel.ForEach(entities.ToArray(), e =>
                {
                    UpdateEntity(e);
                });

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }

        /// <summary>
        /// Updates an entity.
        /// </summary>
        /// <param name="e">The entity to update.</param>
        private static void UpdateEntity(Entity e)
        {
            if (e.Enabled)
            {
                e.Update();
                e.InternalUpdate();
            }
        }

        /// <summary>
        /// Instantiates a new entity in the game.
        /// </summary>
        /// <param name="entity">The entity to instantiate.</param>
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

        public static Entity GetEntityWithUUID(string uuid)
        {
            return entities.FirstOrDefault(e => e.UUID == uuid);
        }
    }

}
