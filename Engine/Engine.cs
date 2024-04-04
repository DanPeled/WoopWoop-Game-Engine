using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Numerics;
using System.Threading.Tasks;
using Raylib_cs;
#if DEBUG
using WoopWoop.Editor;
#endif
namespace WoopWoop
{
    public class WoopWoopEngine
    {
        static Game? game;
        private static readonly object entitiesLock = new();
        private static Stopwatch stopwatch = new(); // Add a Stopwatch for measuring time
        private static List<List<Renderer>> renderBatches; // List of render batches, each for a specific layer
        private static float deltaTime = 0;
        public static readonly int screenWidth = 1080, screenHeight = 720;
        private static int batchSize = 10; // Initial batch size
        private static int maxThreads = Environment.ProcessorCount; // Max threads based on the processor count
        public static bool IsInDebugMenu { get; private set; } = false;
        static Camera mainCamera;
        static Camera debugCamera;
        static Entity[] currentFrameEntities;
        public static string windowTitle = "Game";
        private static void Init(Game game_)
        {
            Raylib.SetTargetFPS(240);
            game = game_;
            renderBatches = new List<List<Renderer>>();
#if DEBUG
            Editor.Editor.Init();
#endif
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
            // Raylib.SetConfigFlags(ConfigFlags.FullscreenMode);
            Raylib.InitWindow(screenWidth, screenHeight, windowTitle);

            Entity camera = new();
            Entity debugCameraEntity = new();
            Entity.Instantiate(camera);
            Entity.Instantiate(debugCameraEntity);
            mainCamera = camera.AddComponent<Camera>();
            mainCamera.IsMain = true;
            debugCamera = debugCameraEntity.AddComponent<Camera>();
            // mainCamera = Camera.GetMainCamera();
            game?.Start();
            currentFrameEntities = Entity.GetAllEntities();
            while (!Raylib.WindowShouldClose())
            {
                if (currentFrameEntities != Entity.GetAllEntities())
                {
                    currentFrameEntities = Entity.GetAllEntities();
                }
                // Measure the deltaTime
                deltaTime = (float)stopwatch.Elapsed.TotalSeconds;
                stopwatch.Restart();

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.White);
                game?.Update();

                HandleDebugMenu();

                Raylib.BeginMode2D(IsInDebugMenu ? debugCamera.camera : mainCamera.camera);

                RenderFrame();
                // Process currentFrameEntities in batches
                UpdateEntities();

                HandleCameraSwitch();

                OnEndOfFrame();

                Raylib.EndMode2D();
                Raylib.EndDrawing();
            }
            foreach (Entity entity in currentFrameEntities.ToArray())
            {
                Entity.Destroy(entity);
            }
            Raylib.CloseWindow();
        }

        private static void UpdateEntity(Entity e)
        {
            if (e.Enabled)
            {
                if (!IsInDebugMenu)
                {
                    e.InternalUpdate(deltaTime);
                }
#if DEBUG
                DrawGizmos(e);
#endif
            }
        }
        private static void HandleDebugMenu()
        {
#if DEBUG
            if (!IsInDebugMenu)
            {
                Editor.Editor.TurnOff();
            }
            if (Raylib.IsKeyPressed(KeyboardKey.F3))
            {
                IsInDebugMenu = !IsInDebugMenu;
            }
            else if (IsInDebugMenu)
            {
                Editor.Editor.DebugMenu();
                DebugRender();
            }
#endif
        }

        private static void HandleCameraSwitch()
        {
            if (mainCamera != Camera.Main() || !mainCamera.IsMain)
            {
                mainCamera = Camera.Main();
            }
        }

        public static void UpdateEntities()
        {
            Parallel.ForEach(GetEntityBatches(), batch =>
                {
                    foreach (Entity entity in batch)
                    {
                        UpdateEntity(entity);
                    }
                });
            for (int i = 0; i < currentFrameEntities.Length; i++)
            {
                UpdateEntity(currentFrameEntities.ElementAt(i));
            }
        }
        private static IEnumerable<List<Entity>> GetEntityBatches()
        {
            int entityCount = currentFrameEntities.Length;
            int batches = (entityCount + batchSize - 1) / batchSize; // Ceiling division to calculate number of batches

            // Calculate the batch size dynamically relative to the total number of currentFrameEntities
            int dynamicBatchSize = (entityCount + batches - 1) / batches;

            for (int i = 0; i < batches; i++)
            {
                int start = i * dynamicBatchSize;
                int count = Math.Min(dynamicBatchSize, entityCount - start);
                yield return currentFrameEntities.ToList().GetRange(start, count);
            }
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

        private static void RenderFrame()
        {
            // Iterate through render batches by layer, rendering each batch
            foreach (var batch in renderBatches)
            {
                foreach (Renderer r in batch)
                {
#if DEBUG
                    if (Editor.Editor.debugMenuEntities.Contains(r.entity))
                    {
                        continue;
                    }
#endif
                    if (r.entity.Enabled && r.Enabled)
                    {
                        r.Update(deltaTime);
                    }
                }
            }
        }


        private static void DrawGizmos(Entity entity)
        {
            if (Editor.Editor.debugMenuEntities.Contains(entity)) return;
            if (!IsInDebugMenu) return;
            foreach (Component c in entity.GetComponents())
            {
                c.OnDrawGizmo();
            }
        }

        public static Entity[] GetEntities()
        {
            return currentFrameEntities.ToArray();
        }

        private static void OnEndOfFrame()
        {
            foreach (Entity e in currentFrameEntities)
            {
                foreach (Component component in e.GetComponents())
                {
                    component.OnEndOfFrame();
                }
            }
        }
#if DEBUG
        private static void DebugRender()
        {
            Editor.Editor.debugMenuEntities.ForEach(UpdateEntity);
            // Iterate through render batches by layer, rendering each batch
            foreach (var batch in renderBatches)
            {
                foreach (Renderer r in batch)
                {
                    if (Editor.Editor.debugMenuEntities.Contains(r.entity) && r.entity.Enabled && r.Enabled)
                    {
                        r.Update(deltaTime);
                    }
                }
            }
        }
#endif
    }
}
