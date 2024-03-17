using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Raylib_cs;

namespace WoopWoop
{
    public class WoopWoopEngine
    {
        static Game? game;
        private static List<Entity> entities;
        private static readonly object entitiesLock = new object();
        private static Stopwatch stopwatch = new Stopwatch(); // Add a Stopwatch for measuring time
        private static List<List<Renderer>> renderBatches; // List of render batches, each for a specific layer
        private static float deltaTime = 0;

        private static int batchSize = 10; // Initial batch size
        private static int maxThreads = Environment.ProcessorCount; // Max threads based on the processor count

        private static void Init(Game game_)
        {
            Raylib.SetTargetFPS(240);
            game = game_;
            entities = new();
            renderBatches = new List<List<Renderer>>();

            // Start the stopwatch
            stopwatch.Start();
        }

        /// <summary>
        /// Starts up the engine, opens the window and starts everything 
        /// </summary>
        /// <param name="game_">Game object reference</param>
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

                // Process entities in batches
                Parallel.ForEach(GetEntityBatches(), batch =>
                {
                    foreach (Entity entity in batch)
                    {
                        UpdateEntity(entity);
                    }
                });

                Render();

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

        private static IEnumerable<List<Entity>> GetEntityBatches()
        {
            int entityCount = entities.Count;
            int batches = (entityCount + batchSize - 1) / batchSize; // Ceiling division to calculate number of batches

            // Calculate the batch size dynamically relative to the total number of entities
            int dynamicBatchSize = (entityCount + batches - 1) / batches;

            for (int i = 0; i < batches; i++)
            {
                int start = i * dynamicBatchSize;
                int count = Math.Min(dynamicBatchSize, entityCount - start);
                yield return entities.GetRange(start, count);
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
            lock (entitiesLock)
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
        }

        private static void StopComponents(Entity entity)
        {
            foreach (Component c in entity.GetComponents())
            {
                entity.RemoveComponent(c.GetType());
            }
        }

        public static Entity GetEntityWithUUID(string uuid)
        {
            return entities.Find(e => e.UUID == uuid);
        }

        public static void AddToRenderBatch(Renderer renderer)
        {
            // Ensure that the renderBatches list has enough elements for each layer
            while (renderBatches.Count <= renderer.Layer)
            {
                renderBatches.Add(new List<Renderer>());
            }
            renderBatches[renderer.Layer].Add(renderer);
        }

        private static void Render()
        {
            // Iterate through render batches by layer, rendering each batch
            foreach (var batch in renderBatches)
            {
                foreach (Renderer r in batch)
                {
                    if (r.entity.Enabled && r.Enabled)
                    {
                        r.Update(deltaTime);
                    }
                }
            }
        }
    }
}
