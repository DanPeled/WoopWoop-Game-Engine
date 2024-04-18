using System.Diagnostics;
using ZeroElectric.Vinculum;


namespace WoopWoop
{
    /// <summary>
    /// The main engine class responsible for managing the game loop and rendering.
    /// </summary>
    public class WoopWoopEngine
    {
        /// <summary>
        /// Gets or sets a value indicating whether the engine is in debug menu mode.
        /// </summary>
        public static bool IsInDebugMenu { get; set; } = false;

        #region Game Data

        /// <summary>
        /// Reference to the current game instance.
        /// </summary>
        static Game? game;

        /// <summary>
        /// The title of the game window.
        /// </summary>
        public static string windowTitle = "Game";

        /// <summary>
        /// The width of the screen.
        /// </summary>
        public static readonly int screenWidth = 800;

        /// <summary>
        /// The height of the screen.
        /// </summary>
        public static readonly int screenHeight = 800;

        #endregion

        #region Update Loop Variables

        /// <summary>
        /// Stopwatch for measuring delta time.
        /// </summary>
        private static Stopwatch stopwatch = new();

        /// <summary>
        /// The time between two consecutive frames.
        /// </summary>
        private static float deltaTime = 0;

        /// <summary>
        /// The size of entity batches for processing.
        /// </summary>
        private static int batchSize = 30; // Initial batch size

        /// <summary>
        /// Thread for updating the game instance.
        /// </summary>
        private static Thread updateGameInstanceThread;

        /// <summary>
        /// Array of entities for the current frame.
        /// </summary>
        private static Entity[] currentFrameEntities;
        private static bool updateEntitiesArray = false;

        #endregion

        #region Rendering

        /// <summary>
        /// The main camera used for rendering.
        /// </summary>
        private static Camera mainCamera;

        /// <summary>
        /// The debug camera used for rendering debug views.
        /// </summary>
        private static Camera debugCamera;

        /// <summary>
        /// The render batch for organizing rendering.
        /// </summary>
        public static rlRenderBatch renderBatch;

        /// <summary>
        /// Dictionary to store render batches by layer.
        /// </summary>
        private static Dictionary<int, List<Renderer>> renderBatches;


        #endregion

        /// <summary>
        /// Initializes the engine.
        /// </summary>
        /// <param name="game_">Reference to the game object.</param>
        private static void Init(Game game_)
        {
            renderBatch.currentDepth = 36;
            renderBatch.currentBuffer = 1;
            game = game_;
            renderBatches = new();

#if DEBUG
            Editor.Editor.Init();
#endif
            // Start the stopwatch
            stopwatch.Start();

            InitEvents();

            InitRaylibSystems();
            InitCamera();
        }

        /// <summary>
        /// Initializes Raylib systems.
        /// </summary>
        private static void InitRaylibSystems()
        {
            int traceLogLevel = 4;
#if !DEBUG
            traceLogLevel = 7;
#endif
            Raylib.SetTraceLogLevel(traceLogLevel);
            Raylib.InitAudioDevice();
            // Raylib.SetConfigFlags(ConfigFlags.FLAG_FULLSCREEN_MODE);
            Raylib.InitWindow(screenWidth, screenHeight, windowTitle);
            Raylib.SetTargetFPS(60);
        }

        /// <summary>
        /// Initializes the main and debug cameras.
        /// </summary>
        private static void InitCamera()
        {
            Entity camera = new();
            Entity debugCameraEntity = new();
            Entity.Instantiate(camera);
            Entity.Instantiate(debugCameraEntity);
            mainCamera = camera.AddComponent<Camera>();
            mainCamera.IsMain = true;
            debugCamera = debugCameraEntity.AddComponent<Camera>();
        }

        /// <summary>
        /// Starts the engine, opens the window, and begins the game loop.
        /// </summary>
        /// <param name="game_">Reference to the game object.</param>
        public static void Start(Game game_)
        {
            Init(game_);


            game?.Start();

            currentFrameEntities = Entity.GetAllEntities();

            updateGameInstanceThread = new Thread(new ThreadStart(() =>
            {
                while (!Raylib.WindowShouldClose())
                {
                    HandleUpdateGameInstance();
                }
            }));
            updateGameInstanceThread.Start();

            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();

                HandleRenderFrame();

                HandleDebugMenu();

                Raylib.EndDrawing();
            }

            Cleanup();
        }

        /// <summary>
        /// Cleans up resources and closes the window.
        /// </summary>
        private static void Cleanup()
        {
            game?.OnStop();

            foreach (Entity entity in currentFrameEntities.ToArray())
            {
                Entity.Destroy(entity);
            }

            Raylib.CloseAudioDevice();
            updateGameInstanceThread.Interrupt();
            Raylib.CloseWindow();
        }

        /// <summary>
        /// Initializes all of the event listeners around the engine.
        /// </summary>
        private static void InitEvents()
        {
            Entity.OnEntityInstantiated += (Entity entity) =>
            {
                updateEntitiesArray = true;
            };
            Entity.OnEntityDestroyed += (Entity entity) =>
            {
                updateEntitiesArray = true;
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
        /// Handles the game update logic.
        /// </summary>
        private static void HandleUpdateGameInstance()
        {
            // Measure the deltaTime
            deltaTime = (float)stopwatch.Elapsed.TotalSeconds;
            stopwatch.Restart();
            game?.Update();
        }

        /// <summary>
        /// Renders the frame.
        /// </summary>
        private static void HandleRenderFrame()
        {
            Raylib.ClearBackground(Camera.Main().backgroundColor);
            Raylib.BeginMode2D(IsInDebugMenu ? debugCamera.camera : mainCamera.camera);

            UpdateEntities();
            RenderFrame();
            HandleCameraSwitch();
            OnEndOfFrame();

            Raylib.EndMode2D();
        }

        /// <summary>
        /// Updates individual entities.
        /// </summary>
        /// <param name="e">The entity to update.</param>
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

        /// <summary>
        /// Handles the debug menu logic.
        /// </summary>
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

        /// <summary>
        /// Handles switching of the main camera.
        /// </summary>
        private static void HandleCameraSwitch()
        {
            if (mainCamera != Camera.Main() || !mainCamera.IsMain)
            {
                mainCamera = Camera.Main();
            }
        }

        /// <summary>
        /// Updates all entities.
        /// </summary>
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

        /// <summary>
        /// Splits entities into batches for parallel processing.
        /// </summary>
        private static IEnumerable<List<Entity>> GetEntityBatches()
        {
            int entityCount = currentFrameEntities.Length;
            int batches = (entityCount + batchSize - 1) / batchSize;

            int dynamicBatchSize = (entityCount + batches - 1) / batches;

            for (int i = 0; i < batches; i++)
            {
                int start = i * dynamicBatchSize;
                int count = Math.Min(dynamicBatchSize, entityCount - start);
                yield return currentFrameEntities.ToList().GetRange(start, count);
            }
        }

        /// <summary>
        /// Adds a renderer to the rendering batch.
        /// </summary>
        /// <param name="renderer">The renderer to add.</param>
        public static void AddToRenderBatch(Renderer renderer)
        {
            if (!renderBatches.ContainsKey(renderer.Layer))
            {
                renderBatches[renderer.Layer] = new List<Renderer>();
            }

            renderBatches[renderer.Layer].Add(renderer);
        }

        /// <summary>
        /// Renders the frame.
        /// </summary>
        private static void RenderFrame()
        {
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
                    if ((r.entity.Enabled && r.Enabled) || IsInDebugMenu)
                    {
                        r.Update(deltaTime);
                    }
                }
            }
        }

        /// <summary>
        /// Draws gizmos for debug rendering.
        /// </summary>
        /// <param name="entity">The entity to draw gizmos for.</param>
        private static void DrawGizmos(Entity entity)
        {
            // Not implemented
        }

        /// <summary>
        /// Invokes the OnEndOfFrame method for all components on all entities.
        /// </summary>
        private static void OnEndOfFrame()
        {
            foreach (Entity e in currentFrameEntities)
            {
                foreach (Component component in e.GetComponents())
                {
                    component.OnEndOfFrame();
                }
            }
            if (updateEntitiesArray)
            {
                currentFrameEntities = Entity.GetAllEntities();
                updateEntitiesArray = false;
            }
        }

#if DEBUG
        /// <summary>
        /// Renders debug entities.
        /// </summary>
        private static void DebugRender()
        {
            Editor.Editor.debugMenuEntities.ForEach(UpdateEntity);

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

        /// <summary>
        /// Changes the renderer's layer and updates render batches.
        /// </summary>
        /// <param name="r">The renderer to update.</param>
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
