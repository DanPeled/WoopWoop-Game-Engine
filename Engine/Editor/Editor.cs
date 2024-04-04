using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Raylib_cs;
using WoopWoop.UI;
namespace WoopWoop.Editor
{
    public static class Editor
    {
        public enum EditorState
        {
            Pos, Scale, Rotation
        }
        public static EditorState editorState;
        private static Entity selectedEntity;
        public static Entity SelectedEntity
        {
            get { return selectedEntity; }
            set
            {
                selectedEntity = value;
                UpdateText();
            }
        }
        public static List<Entity> debugMenuEntities = new();
        private static Entity editorWindow = new();
        private static TextRenderer UUIDText, positionText, scaleText, angleText, componentsText;
        public static void Init()
        {
            BasicShapeRenderer renderer = new()
            {
                shape = BasicShape.Box,
                Color = new Color(130, 130, 130, 200),
                Layer = 254
            };
            editorWindow.AddComponent(renderer);
            editorWindow.transform.pivot = Pivot.TopLeft;
            editorWindow.transform.Scale = new(45, 50);
            editorWindow.transform.Position = new(WoopWoopEngine.screenWidth - (editorWindow.transform.Scale.X * 10 - 5), 0);


            Entity uuidTextEntity = new();
            UUIDText = uuidTextEntity.AddComponent<TextRenderer>();
            uuidTextEntity.transform.Position = new(WoopWoopEngine.screenWidth - (editorWindow.transform.Scale.X * 10 - 17), 12);
            UUIDText.text = "UUID: ";
            UUIDText.Layer = 255;

            Entity positionTextEntity = new();
            positionText = positionTextEntity.AddComponent<TextRenderer>();
            positionTextEntity.transform.Position = new(WoopWoopEngine.screenWidth - (editorWindow.transform.Scale.X * 10 - 17), 26);
            positionText.text = "Pos: ";
            positionText.Color = Color.Black;
            positionText.Layer = 255;


            Entity scaleTextEntity = new();
            scaleText = scaleTextEntity.AddComponent<TextRenderer>();
            scaleTextEntity.transform.Position = new(WoopWoopEngine.screenWidth - (editorWindow.transform.Scale.X * 10 - 17), 70);
            scaleText.text = "Scale: ";
            scaleText.Color = Color.Black;
            scaleText.Layer = 255;

            Entity angleTextEntity = new();
            angleText = angleTextEntity.AddComponent<TextRenderer>();
            angleTextEntity.transform.Position = new(WoopWoopEngine.screenWidth - (editorWindow.transform.Scale.X * 10 - 17), 130);
            angleText.text = "Angle: ";
            angleText.Color = Color.Black;
            angleText.Layer = 255;

            Entity compoenetsTextEntity = new();
            componentsText = compoenetsTextEntity.AddComponent<TextRenderer>();
            compoenetsTextEntity.transform.Position = new(WoopWoopEngine.screenWidth - (editorWindow.transform.Scale.X * 10 - 17), 170);
            componentsText.text = "Components: ";
            componentsText.Color = Color.Black;
            componentsText.Layer = 255;

            editorWindow.Enabled = false;



            editorWindow.RemoveComponent<PointCollider>();
            Entity.Instantiate(editorWindow);
            Entity.Instantiate(uuidTextEntity);
            Entity.Instantiate(positionTextEntity);
            Entity.Instantiate(compoenetsTextEntity);
            Entity.Instantiate(angleTextEntity);
            Entity.Instantiate(scaleTextEntity);

            editorWindow.transform.AddChild(uuidTextEntity);
            editorWindow.transform.AddChild(positionTextEntity);
            editorWindow.transform.AddChild(angleTextEntity);
            editorWindow.transform.AddChild(scaleTextEntity);
            editorWindow.transform.AddChild(compoenetsTextEntity);


            List<Entity> entities = new() { editorWindow };
            entities.AddRange(editorWindow.transform.GetChildren().ToList().Select(t => t.entity));
            debugMenuEntities.AddRange(entities);

        }
        public static void DrawMenu()
        {
            editorWindow.Enabled = true;
            UpdateText();
        }
        public static void TurnOff()
        {
            editorWindow.Enabled = false;
        }
        public static void UpdateText()
        {
            if (UUIDText != null && selectedEntity != null)
            {
                UUIDText.text = $"UUID: {SelectedEntity.ID}";
                positionText.text = $"Pos: \n\tX:  {selectedEntity.transform.Position.X}\n\tY:  {selectedEntity.transform.Position.Y}";
                scaleText.text = $"Scale: \n\tX:  {selectedEntity.transform.Scale.X}\n\tY:  {selectedEntity.transform.Scale.Y}";
                angleText.text = $"Angle: {selectedEntity.transform.Angle}";
                string componentsString = "";

                foreach (Component component in selectedEntity.GetComponents())
                {

                    if (component.GetType() != typeof(PointCollider))
                        componentsString += component.GetType().Name + ", ";
                }
                componentsText.text = $"Components: {componentsString}" + " " + selectedEntity.Name;
            }
        }
        public static void DebugMenu()
        {
            if (!WoopWoopEngine.IsInDebugMenu) return;

            // Check if the left mouse button is pressed
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                SelectedEntity = null;
                // Get the mouse position
                Vector2 mousePosition = Raylib.GetMousePosition();
                // Iterate through entities to check for collision with the mouse position
                foreach (Entity entity in Entity.GetAllEntities())
                {
                    PointCollider collider = entity.GetComponent<PointCollider>();
                    if (collider != null)
                    {
                        if (collider.CheckCollision(mousePosition))
                        {
                            // Store the selected entity
                            SelectedEntity = entity;
                            break; // Exit the loop after finding the first entity
                        }
                    }
                }
            }

            // Check if the left mouse button is being held down and a draggable entity is selected
            if (Raylib.IsMouseButtonDown(MouseButton.Left) && SelectedEntity != null)
            {
                float speedScalar = 1f;
                if (Raylib.IsKeyDown(KeyboardKey.LeftShift))
                {
                    speedScalar = 7f;
                }
                switch (editorState)
                {
                    case EditorState.Pos:
                    default:
                        {
                            // Update the position of the selected entity based on the mouse movement
                            Vector2 mousePos = Raylib.GetMousePosition();
                            Vector2 newPos = mousePos;
                            if (Raylib.IsKeyDown(KeyboardKey.LeftAlt))
                            {
                                if (Raylib.IsKeyDown(KeyboardKey.X))
                                {
                                    newPos.Y = SelectedEntity.transform.Position.Y;
                                    Raylib.DrawLineEx(new(-100, newPos.Y), new(WoopWoopEngine.screenWidth + 100, newPos.Y), 3, new Color(255, 0, 0, 200));

                                }
                                else if (Raylib.IsKeyDown(KeyboardKey.Y))
                                {
                                    newPos.X = SelectedEntity.transform.Position.X;
                                    Raylib.DrawLineEx(new(newPos.X, -100), new(newPos.X, WoopWoopEngine.screenHeight + 100), 3, new Color(0, 255, 0, 200));
                                }
                            }
                            SelectedEntity.transform.Position = newPos;
                            break;
                        }
                    case EditorState.Rotation:
                        {
                            Entity selectedEntity = SelectedEntity;
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
                    case EditorState.Scale:
                        {
                            Vector2 mousePos = Raylib.GetMouseDelta() / 30 / speedScalar;
                            Vector2 newScale = mousePos;
                            if (Raylib.IsKeyDown(KeyboardKey.LeftAlt))
                            {
                                if (Raylib.IsKeyDown(KeyboardKey.X))
                                {
                                    newScale.Y = SelectedEntity.transform.Position.Y;
                                    Raylib.DrawLineEx(new(-100, newScale.Y), new(WoopWoopEngine.screenWidth + 100, newScale.Y), 3, new Color(255, 0, 0, 200));

                                }
                                else if (Raylib.IsKeyDown(KeyboardKey.Y))
                                {
                                    newScale.X = SelectedEntity.transform.Position.X;
                                    Raylib.DrawLineEx(new(newScale.X, -100), new(newScale.X, WoopWoopEngine.screenHeight + 100), 3, new Color(0, 255, 0, 200));
                                }
                            }
                            KeepMouseInScreen();
                            SelectedEntity.transform.Scale += newScale;
                            break;
                        }

                }
            }
            else if (Raylib.IsKeyDown(KeyboardKey.R))
            {
                editorState = EditorState.Rotation;
            }
            else if (Raylib.IsKeyDown(KeyboardKey.G))
            {
                editorState = EditorState.Pos;
            }
            else if (Raylib.IsKeyDown(KeyboardKey.S))
            {
                editorState = EditorState.Scale;
            }

            DrawMenu();
        }
        private static void KeepMouseInScreen()
        {
            if (Raylib.GetMousePosition().X >= WoopWoopEngine.screenWidth)
            {
                Raylib.SetMousePosition(0, (int)Raylib.GetMousePosition().Y);
            }
            else if (Raylib.GetMousePosition().X <= 0)
            {
                Raylib.SetMousePosition(WoopWoopEngine.screenWidth, (int)Raylib.GetMousePosition().Y);
            }
            else if (Raylib.GetMousePosition().Y >= WoopWoopEngine.screenHeight)
            {
                Raylib.SetMousePosition((int)Raylib.GetMousePosition().X, 0);
            }
            else if (Raylib.GetMousePosition().Y <= 0)
            {
                Raylib.SetMousePosition((int)Raylib.GetMousePosition().X, WoopWoopEngine.screenHeight);
            }
        }
    }

}
