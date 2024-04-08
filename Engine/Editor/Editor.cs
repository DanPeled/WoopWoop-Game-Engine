using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;
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
        private static TextRenderer UUIDText, positionText, scaleText, angleText;
        private static DropdownMenu dropdownMenu;
        public static Entity cursor;

        public static void Init()
        {
            cursor = Entity.CreateEntity().AddComponent<BoxCollider>().SetScale(new(1, 1)).SetPosition(new(0, 0)).Create();

            // BasicShapeRenderer renderer = new()s
            editorWindow.AddComponent<UIWindow>();
            editorWindow.transform.pivot = Pivot.TopLeft;
            editorWindow.transform.Scale = new(450, WoopWoopEngine.screenHeight - 20);
            editorWindow.transform.Position = new(10, 10);
            float xPos = 13;

            Entity uuidTextEntity = new();
            UUIDText = uuidTextEntity.AddComponent<TextRenderer>();
            uuidTextEntity.transform.Position = new(xPos, 12);
            UUIDText.text = "UUID: ";
            UUIDText.Layer = 255;
            // uuidTextEntity.transform.Scale = new(100, 100);

            Entity positionTextEntity = new();
            positionText = positionTextEntity.AddComponent<TextRenderer>();
            positionTextEntity.transform.Position = new(xPos, 12);
            positionText.text = "Pos: ";
            positionText.Color = Raylib.BLACK;
            positionText.Layer = 255;


            Entity scaleTextEntity = new();
            scaleText = scaleTextEntity.AddComponent<TextRenderer>();
            scaleTextEntity.transform.Position = new(xPos, 12);
            scaleText.text = "Scale: ";
            scaleText.Color = Raylib.BLACK;
            scaleText.Layer = 255;

            Entity angleTextEntity = new();
            angleText = angleTextEntity.AddComponent<TextRenderer>();
            angleTextEntity.transform.Position = new(xPos, 12);
            angleText.text = "Angle: ";
            angleText.Color = Raylib.BLACK;
            angleText.Layer = 255;

            editorWindow.AddComponent<VerticalGrid>().spacing = 50;

            editorWindow.Enabled = false;

            Entity dropdown = Entity.CreateEntity()
            .AddComponent<DropdownMenu>()
            .SetScale(new(120, 100)).SetPosition(new(xPos, 12))
            .Create();
            dropdown.RemoveComponent<PointCollider>();
            dropdownMenu = dropdown.GetComponent<DropdownMenu>();

            editorWindow.RemoveComponent<PointCollider>();
            Entity.Instantiate(editorWindow);
            Entity.Instantiate(dropdown);
            Entity.Instantiate(uuidTextEntity);
            Entity.Instantiate(positionTextEntity);
            Entity.Instantiate(angleTextEntity);
            Entity.Instantiate(scaleTextEntity);

            editorWindow.transform.AddChild(uuidTextEntity);
            editorWindow.transform.AddChild(positionTextEntity);
            editorWindow.transform.AddChild(angleTextEntity);
            editorWindow.transform.AddChild(scaleTextEntity);
            editorWindow.transform.AddChild(dropdown);

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
                Component[] components = selectedEntity.GetComponents();
                for (int i = 0; i < components.Length; i++)
                {
                    Component component = components[i];
                    if (component.GetType() != typeof(PointCollider) && component.GetType() != typeof(Transform))
                        componentsString += component.GetType().Name + (i < components.Length - 1 ? "\n" : "");
                }

                dropdownMenu.itemsString = componentsString;
            }
        }
        public static void DebugMenu()
        {
            cursor.transform.Position = Raylib.GetMousePosition();
            if (!WoopWoopEngine.IsInDebugMenu) return;
            foreach (Transform child in editorWindow.transform.GetChildren())
            {
                child.entity.Enabled = true;
            }
            // Check if the left mouse button is pressed
            if (Raylib.IsMouseButtonPressed(Raylib.MOUSE_LEFT_BUTTON))
            {
                SelectedEntity = null;
                // Get the mouse position
                Vector2 mousePosition = Raylib.GetMousePosition();
                // Iterate through entities to check for collision with the mouse position
                // List<Entity> collidedEntities = new();
                foreach (Entity entity in Entity.GetAllEntities())
                {
                    BoxCollider collider = entity.GetComponent<BoxCollider>();
                    if (collider != null)
                    {
                        if (collider.IsCollidingWith(cursor.GetComponent<BoxCollider>()))
                        {
                            // Store the selected entity
                            SelectedEntity = entity;
                            // collidedEntities.Add(collider.entity);
                            break; // Exit the loop after finding the first entity
                        }
                    }
                }

            }

            // Check if the left mouse button is being held down and a draggable entity is selected
            if (Raylib.IsMouseButtonDown(Raylib.MOUSE_LEFT_BUTTON) && SelectedEntity != null)
            {
                float speedScalar = 1f;
                if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT))
                {
                    speedScalar = 7f;
                }
                switch (editorState)
                {
                    case EditorState.Pos:
                    default:
                        {
                            // Update the position of the selected entity based on the mouse movement
                            Vector2 mousePos = Raylib.GetMousePosition() - new Vector2(500, 0);
                            Vector2 newPos = mousePos;
                            if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_ALT))
                            {
                                if (Raylib.IsKeyDown(KeyboardKey.KEY_X))
                                {
                                    newPos.Y = SelectedEntity.transform.Position.Y;
                                    Raylib.DrawLineEx(new(-100, newPos.Y), new(WoopWoopEngine.screenWidth + 100, newPos.Y), 3, new Color(255, 0, 0, 200));

                                }
                                else if (Raylib.IsKeyDown(KeyboardKey.KEY_Y))
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
                            if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_ALT))
                            {
                                if (Raylib.IsKeyDown(KeyboardKey.KEY_X))
                                {
                                    newScale.Y = SelectedEntity.transform.Position.Y;
                                    Raylib.DrawLineEx(new(-100, newScale.Y), new(WoopWoopEngine.screenWidth + 100, newScale.Y), 3, new Color(255, 0, 0, 200));

                                }
                                else if (Raylib.IsKeyDown(KeyboardKey.KEY_Y))
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
            else if (Raylib.IsKeyDown(KeyboardKey.KEY_R))
            {
                editorState = EditorState.Rotation;
            }
            else if (Raylib.IsKeyDown(KeyboardKey.KEY_G))
            {
                editorState = EditorState.Pos;
            }
            else if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
            {
                editorState = EditorState.Scale;
            }

            DrawMenu();
        }
        private static void KeepMouseInScreen()
        {
            if (Raylib.GetMousePosition().X >= WoopWoopEngine.screenWidth - 2)
            {
                Raylib.SetMousePosition(0, (int)Raylib.GetMousePosition().Y);
            }
            else if (Raylib.GetMousePosition().X <= 2)
            {
                Raylib.SetMousePosition(WoopWoopEngine.screenWidth, (int)Raylib.GetMousePosition().Y);
            }
            else if (Raylib.GetMousePosition().Y >= WoopWoopEngine.screenHeight - 2)
            {
                Raylib.SetMousePosition((int)Raylib.GetMousePosition().X, 0);
            }
            else if (Raylib.GetMousePosition().Y <= 2)
            {
                Raylib.SetMousePosition((int)Raylib.GetMousePosition().X, WoopWoopEngine.screenHeight);
            }
        }
    }

}
