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
        private static List<Entity> entities;
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
        public static string windowTitle = "Game";
        private static void Init(Game game_)
        {
            Raylib.SetTargetFPS(240);
            game = game_;
            entities = new();
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

            Raylib.InitWindow(screenWidth, screenHeight, windowTitle);
            Entity camera = new();
            Entity debugCameraEntity = new();
            Instantiate(camera);
            Instantiate(debugCameraEntity);
            mainCamera = camera.AddComponent<Camera>();
            mainCamera.IsMain = true;
            debugCamera = debugCameraEntity.AddComponent<Camera>();
            // mainCamera = Camera.GetMainCamera();
            game?.Start();
            while (!Raylib.WindowShouldClose())
            {
                // Measure the deltaTime
                deltaTime = (float)stopwatch.Elapsed.TotalSeconds;
                stopwatch.Restart();

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.White);
                game?.Update();

                HandleDebugMenu();

                Raylib.BeginMode2D(IsInDebugMenu ? debugCamera.camera : mainCamera.camera);

                RenderFrame();
                // Process entities in batches
                UpdateEntities();

                HandleCameraSwitch();

                OnEndOfFrame();

                Raylib.EndMode2D();
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
                DebugMenu();
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
                component.Awake();
                if (component.Enabled)
                {
                    component.Start();
                }
            }
        }

        public static void Destroy(Entity entity)
        {
            lock (entitiesLock)
            {
                var entitiesCopy = new List<Entity>(entities);
                if (entity.transform != null)
                {
                    foreach (Transform child in entity.transform.GetChildren())
                    {
                        if (child.entity != null)
                        {
                            StopComponents(child.entity);
                            entitiesCopy.Remove(child.entity);
                        }
                    }
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
            return entities.Find(e => e.ID.Equals(uuid));
        }
        public static Entity[] GetEntitiesWithTag(string tag)
        {
            return entities.FindAll(e => e.tag.Equals(tag)).ToArray();
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
        private static void DebugMenu()
        {
            if (!IsInDebugMenu) return;

            // Check if the left mouse button is pressed
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                Editor.Editor.SelectedEntity = null;
                // Get the mouse position
                Vector2 mousePosition = Raylib.GetMousePosition();
                // Iterate through entities to check for collision with the mouse position
                foreach (Entity entity in entities)
                {
                    PointCollider collider = entity.GetComponent<PointCollider>();
                    if (collider != null)
                    {
                        if (collider.CheckCollision(mousePosition))
                        {
                            // Store the selected entity
                            Editor.Editor.SelectedEntity = entity;
                            break; // Exit the loop after finding the first entity
                        }
                    }
                }
            }

            // Check if the left mouse button is being held down and a draggable entity is selected
            if (Raylib.IsMouseButtonDown(MouseButton.Left) && Editor.Editor.SelectedEntity != null)
            {
                float speedScalar = 1f;
                if (Raylib.IsKeyDown(KeyboardKey.LeftShift))
                {
                    speedScalar = 7f;
                }
                switch (Editor.Editor.editorState)
                {
                    case Editor.Editor.EditorState.Pos:
                    default:
                        {
                            // Update the position of the selected entity based on the mouse movement
                            Vector2 mousePos = Raylib.GetMousePosition();
                            Vector2 newPos = mousePos;
                            if (Raylib.IsKeyDown(KeyboardKey.LeftAlt))
                            {
                                if (Raylib.IsKeyDown(KeyboardKey.X))
                                {
                                    newPos.Y = Editor.Editor.SelectedEntity.transform.Position.Y;
                                    Raylib.DrawLineEx(new(-100, newPos.Y), new(screenWidth + 100, newPos.Y), 3, new Color(255, 0, 0, 200));

                                }
                                else if (Raylib.IsKeyDown(KeyboardKey.Y))
                                {
                                    newPos.X = Editor.Editor.SelectedEntity.transform.Position.X;
                                    Raylib.DrawLineEx(new(newPos.X, -100), new(newPos.X, screenHeight + 100), 3, new Color(0, 255, 0, 200));
                                }
                            }
                            Editor.Editor.SelectedEntity.transform.Position = newPos;
                            break;
                        }
                    case Editor.Editor.EditorState.Rotation:
                        {
                            Entity selectedEntity = Editor.Editor.SelectedEntity;
                            Transform transform = selectedEntity.transform;

                            // Calculate the angle between the mouse position and the center of the object
                            float angle = transform.Angle;
                            Vector2 objectCenter = transform.Position;
                            Vector2 mouseDelta = Raylib.GetMouseDelta();
                            float rotationSpeed = 1.0f * speedScalar; // Adjust rotation speed as needed

                            // Calculate the rotation angle based on mouse movement
                            angle += mouseDelta.X / rotationSpeed;

                            // Update the object's angle
                            transform.Angle = angle;
                            KeepMouseInScreen();
                            break;
                        }
                    case Editor.Editor.EditorState.Scale:
                        {
                            Vector2 mousePos = Raylib.GetMouseDelta() / 30 / speedScalar;
                            Vector2 newScale = mousePos;
                            if (Raylib.IsKeyDown(KeyboardKey.LeftAlt))
                            {
                                if (Raylib.IsKeyDown(KeyboardKey.X))
                                {
                                    newScale.Y = Editor.Editor.SelectedEntity.transform.Position.Y;
                                    Raylib.DrawLineEx(new(-100, newScale.Y), new(screenWidth + 100, newScale.Y), 3, new Color(255, 0, 0, 200));

                                }
                                else if (Raylib.IsKeyDown(KeyboardKey.Y))
                                {
                                    newScale.X = Editor.Editor.SelectedEntity.transform.Position.X;
                                    Raylib.DrawLineEx(new(newScale.X, -100), new(newScale.X, screenHeight + 100), 3, new Color(0, 255, 0, 200));
                                }
                            }
                            KeepMouseInScreen();
                            Editor.Editor.SelectedEntity.transform.Scale += newScale;
                            break;
                        }

                }
            }
            else if (Raylib.IsKeyDown(KeyboardKey.R))
            {
                Editor.Editor.editorState = Editor.Editor.EditorState.Rotation;
            }
            else if (Raylib.IsKeyDown(KeyboardKey.G))
            {
                Editor.Editor.editorState = Editor.Editor.EditorState.Pos;
            }
            else if (Raylib.IsKeyDown(KeyboardKey.S))
            {
                Editor.Editor.editorState = Editor.Editor.EditorState.Scale;
            }

            Editor.Editor.DrawMenu();
        }

        private static void DrawGizmos(Entity entity)
        {
            if (Editor.Editor.debugMenuEntities.Contains(entity)) return;
            // if (!IsInDebugMenu) return;
            foreach (Component c in entity.GetComponents())
            {
                c.OnDrawGizmo();
            }
        }

        public static Entity[] GetEntities()
        {
            return entities.ToArray();
        }
        private static void KeepMouseInScreen()
        {
            if (Raylib.GetMousePosition().X >= screenWidth)
            {
                Raylib.SetMousePosition(0, (int)Raylib.GetMousePosition().Y);
            }
            else if (Raylib.GetMousePosition().X <= 0)
            {
                Raylib.SetMousePosition(screenWidth, (int)Raylib.GetMousePosition().Y);
            }
            else if (Raylib.GetMousePosition().Y >= screenHeight)
            {
                Raylib.SetMousePosition((int)Raylib.GetMousePosition().X, 0);
            }
            else if (Raylib.GetMousePosition().Y <= 0)
            {
                Raylib.SetMousePosition((int)Raylib.GetMousePosition().X, screenHeight);
            }
        }
        private static void OnEndOfFrame()
        {
            foreach (Entity e in entities)
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
