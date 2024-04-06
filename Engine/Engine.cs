using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Numerics;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;
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
        private static Dictionary<int, List<Renderer>> renderBatches; // Dictionary to store render batches by layer
        private static float deltaTime = 0;
        public static readonly int screenWidth = 1080, screenHeight = 720;
        private static int batchSize = 10; // Initial batch size
        private static int maxThreads = Environment.ProcessorCount; // Max threads based on the processor count
        public static bool IsInDebugMenu { get; private set; } = false;
        static Camera mainCamera;
        static Camera debugCamera;
        static Entity[] currentFrameEntities;
        public static rlRenderBatch renderBatch;
        public static string windowTitle = "Game";
        private static void Init(Game game_)
        {
            renderBatch.currentDepth = 36;
            renderBatch.currentBuffer = 1;
            Raylib.SetTargetFPS(60);
            game = game_;
            renderBatches = new();
#if DEBUG
            Editor.Editor.Init();
#endif
            // Start the stopwatch
            stopwatch.Start();

            Entity.OnEntityDestroyed += (Entity entity) =>
            {
                foreach (int layer in renderBatches.Keys)
                {
                    if (renderBatches[layer].Contains(entity.GetComponent<Renderer>()))
                    {
                        renderBatches[layer].Remove(entity.GetComponent<Renderer>());
                    }
                }
            };
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
                Raylib.ClearBackground(Camera.Main().backgroundColor);
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
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_F3))
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
            // Ensure that the renderBatches dictionary has the key for the renderer's layer
            if (!renderBatches.ContainsKey(renderer.Layer))
            {
                renderBatches[renderer.Layer] = new List<Renderer>();
            }

            renderBatches[renderer.Layer].Add(renderer);
        }


        private static void RenderFrame()
        {
            // Iterate through render batches by layer, rendering each batch
            foreach (var layer in renderBatches.Keys.OrderBy(k => k))
            {
                foreach (Renderer r in renderBatches[layer])
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
            foreach (int layer in renderBatches.Keys)
            {
                foreach (Renderer r in renderBatches[layer])
                {
                    if (Editor.Editor.debugMenuEntities.Contains(r.entity) && r.entity.Enabled && r.Enabled)
                    {
                        r.Update(deltaTime);
                    }
                }
            }
        }
#endif

        public static void ChangeRenderLayer(Renderer r)
        {
            foreach (int layer in renderBatches.Keys)
            {
                if (renderBatches[layer].Contains(r))
                {
                    renderBatches[layer].Remove(r);
                    break;
                }
            }
            AddToRenderBatch(r);
        }
    }
}
