using System.Numerics;
using ZeroElectric.Vinculum;
using WoopWoop.UI;
using System.Reflection;
using WoopWoop.Engine;
namespace WoopWoop.Editor
{
    public class Editor : Subsystem
    {
        /// <summary>
        /// Tells the engine wheter to use the subsystem on release or not
        /// </summary>
        public override bool DebugModeOnly { get; set; } = true;
        /// <summary>
        /// The cursor entity used in the editor.
        /// </summary>
        public static Entity cursor;

        /// <summary>
        /// List of entities in the debug menu.
        /// </summary>
        public static List<Entity> debugMenuEntities { get; private set; } = new();

        /// <summary>
        /// The currently selected entity in the editor.
        /// </summary>
        public static Entity SelectedEntity
        {
            get { return selectedEntity; }
            set
            {
                selectedEntity = value;
                selectedEntityMarker.Enabled = selectedEntity == null;
                selectedEntityMarker.transform.Scale = selectedEntity == null ? new Vector2(0, 0) : selectedEntity.transform.Scale;
                UpdateText();
            }
        }

        /// <summary>
        /// The state of the editor.
        /// </summary>
        public static EditorState editorState;

        // Private fields with documentation comments

        /// <summary>
        /// The currently selected entity.
        /// </summary>
        private static Entity selectedEntity;

        /// <summary>
        /// The line used to separate editor windows.
        /// </summary>
        private static Entity seperatingLine;

        /// <summary>
        /// The main editor window.
        /// </summary>
        private static Entity editorWindow = new();

        /// <summary>
        /// Text renderer for displaying UUID.
        /// </summary>
        private static TextRenderer UUIDText;

        /// <summary>
        /// Text renderer for displaying position.
        /// </summary>
        private static TextRenderer positionText;

        /// <summary>
        /// Text renderer for displaying scale.
        /// </summary>
        private static TextRenderer scaleText;

        /// <summary>
        /// Text renderer for displaying angle.
        /// </summary>
        private static TextRenderer angleText;

        /// <summary>
        /// Text renderer for displaying component data.
        /// </summary>
        private static TextRenderer componentDataText;

        /// <summary>
        /// Dropdown menu for selecting components.
        /// </summary>
        private static DropdownMenu dropdownMenu;

        /// <summary>
        /// Checkbox for enabling/disabling entities.
        /// </summary>
        private static Checkbox entityEnabledButton;

        /// <summary>
        /// Transform for representing the selected entity.
        /// </summary>
        private static Transform selectedEntityMarker;

        /// <summary>
        /// Index of the selected component.
        /// </summary>
        private static int selectedComponentIndex = 0;

        /// <summary>
        /// X-coordinate of the dividing line.
        /// </summary>
        public static readonly int dividingLineX = 500;

        /// <summary>
        /// Initializes the editor.
        /// </summary>
        public override void Init()
        {
            cursor = Entity.CreateEntity().AddComponent<BoxCollider2D>().SetScale(new(1, 1)).SetPosition(new(0, 0)).Create();
            seperatingLine = Entity.CreateEntity().SetPosition(new(498, 0)).AddComponent<LineRenderer>().Create();
            seperatingLine.GetComponent<LineRenderer>().thickness = 1;
            seperatingLine.GetComponent<LineRenderer>().isEndPositionRelative = false;
            seperatingLine.GetComponent<LineRenderer>().endPosition = new(498, 1080);
            Entity.Instantiate(seperatingLine);

            selectedEntityMarker = Entity.CreateEntity().AddComponent<PolygonRenderer>().SetPosition(new(0, 0)).SetEnabled(false).SetScale(10, 10).Create().transform;
            selectedEntityMarker.entity.GetComponent<PolygonRenderer>().Color = new Color(0, 0, 0, 255);
            selectedEntityMarker.entity.GetComponent<PolygonRenderer>().Layer = 255;
            selectedEntityMarker.entity.GetComponent<PolygonRenderer>().vertices = PolygonRenderer.BasicShapes.SquareVertices;
            selectedEntityMarker.entity.GetComponent<PolygonRenderer>().thickness = 3;

            // BasicShapeRenderer renderer = new()s
            editorWindow.AddComponent<UIWindow>();
            editorWindow.transform.pivot = Pivot.TopLeft;
            editorWindow.transform.Scale = new(480, WoopWoopEngine.screenHeight - 20);
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

            Entity componentsDataLabel = Entity.CreateEntity().SetScale(1, 1).SetPosition(new(xPos, 12)).AddComponent<TextRenderer>().Create();
            componentsDataLabel.GetComponent<TextRenderer>().text = "Components Data: ";


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

            editorWindow.AddComponent<VerticalGrid>().Spacing = 70;

            editorWindow.Enabled = false;

            Entity dropdown = Entity.CreateEntity()
            .AddComponent<DropdownMenu>()
            .SetScale(new(120, 100)).SetPosition(new(xPos, 12))
            .Create();
            dropdown.RemoveComponent<PointCollider>();
            dropdownMenu = dropdown.GetComponent<DropdownMenu>();
            dropdownMenu.OnSelectionChanged += (int selection) =>
            {
                selectedComponentIndex = selection;
                // Console.WriteLine(selectedComponentIndex);
            };

            Entity enabledButtonEntity = Entity.CreateEntity()
            .AddComponent<Checkbox>().SetPosition(new(400, 12)).SetScale(40, 40).Create();
            entityEnabledButton = enabledButtonEntity.GetComponent<Checkbox>();
            entityEnabledButton.OnIsCheckedChanged += (bool isChecked) =>
            {
                if (SelectedEntity != null)
                    SelectedEntity.Enabled = isChecked;
            };

            Entity componentDataEntity = Entity.CreateEntity().AddComponent<TextRenderer>().SetPosition(dropdown.transform.Position).Create();
            componentDataText = componentDataEntity.GetComponent<TextRenderer>();

            editorWindow.RemoveComponent<PointCollider>();
            Entity.Instantiate(editorWindow);
            Entity.Instantiate(dropdown);
            Entity.Instantiate(uuidTextEntity);
            Entity.Instantiate(positionTextEntity);
            Entity.Instantiate(angleTextEntity);
            Entity.Instantiate(scaleTextEntity);
            Entity.Instantiate(enabledButtonEntity);
            Entity.Instantiate(componentsDataLabel);
            Entity.Instantiate(componentDataEntity);

            editorWindow.transform.AddChild(uuidTextEntity);
            editorWindow.transform.AddChild(positionTextEntity);
            editorWindow.transform.AddChild(angleTextEntity);
            editorWindow.transform.AddChild(scaleTextEntity);
            editorWindow.transform.AddChild(componentsDataLabel);
            editorWindow.transform.AddChild(dropdown);
            editorWindow.transform.AddChild(componentDataEntity);

            List<Entity> entities = new() { editorWindow };
            entities.AddRange(editorWindow.transform.GetChildren().ToList().Select(t => t.entity));
            entities.Add(enabledButtonEntity);
            entities.Add(seperatingLine);
            debugMenuEntities.AddRange(entities);
        }

        /// <summary>
        /// Draws the editor menu.
        /// </summary>
        public static void DrawMenu()
        {
            editorWindow.Enabled = true;
            entityEnabledButton.entity.Enabled = editorWindow.Enabled;
            seperatingLine.Enabled = editorWindow.Enabled;
            UpdateText();
            HandleUpdate();
        }

        /// <summary>
        /// Turns off the editor.
        /// </summary>
        public static void TurnOff()
        {
            editorWindow.Enabled = false;
            entityEnabledButton.entity.Enabled = editorWindow.Enabled;
            seperatingLine.Enabled = editorWindow.Enabled;
        }

        /// <summary>
        /// Updates the text displayed in the editor.
        /// </summary>
        public static void UpdateText()
        {
            if (selectedEntity != null)
            {
                UUIDText.text = $"UUID: {SelectedEntity.ID}";
                positionText.text = $"Pos: \n\tX:  {SelectedEntity.transform.Position.X}\n\tY:  {SelectedEntity.transform.Position.Y}";
                scaleText.text = $"Scale: \n\tX:  {SelectedEntity.transform.Scale.X}\n\tY:  {SelectedEntity.transform.Scale.Y}";
                angleText.text = $"Angle: {SelectedEntity.transform.Angle}";
                string componentsString = "";
                Component[] components = SelectedEntity.GetComponents();
                Component selectedComponent = null;
                List<Component> choosableComponens = new();
                for (int i = 0; i < components.Length; i++)
                {
                    Component component = components[i];
                    if (component.GetType() != typeof(PointCollider) && component.GetType() != typeof(Transform))
                    {
                        componentsString += component.GetType().Name + (i < components.Length - 1 ? "\n" : "");
                        choosableComponens.Add(component);
                    }
                }
                for (int i = 0; i < choosableComponens.Count; i++)
                {
                    if (i == selectedComponentIndex)
                    {
                        Component component = choosableComponens[i];
                        selectedComponent = component;
                    }
                }
                dropdownMenu.ItemsString = componentsString;

                #region Component Data Text
                if (selectedComponent != null)
                {
                    string dataText = "";
                    PropertyInfo[] props = selectedComponent.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    for (int i = 0; i < props.Length; i++)
                    {
                        if (props[i].CanWrite)
                        {
                            object? value = props[i].GetValue(selectedComponent);
                            if (value != null)
                            {
                                if (props[i].PropertyType == typeof(Color))
                                {
                                    Color color = (Color)value;
                                    string colorData = $"{color.r}, {color.g}, {color.b}, {color.a}";
                                    dataText += $"{props[i].Name}: {colorData}\n";
                                }
                                else
                                {
                                    string data = value.ToString();
                                    dataText += $"{props[i].Name}: {data}\n";
                                }
                            }
                        }
                    }
                    componentDataText.text = dataText;
                }
                else
                {
                    componentDataText.text = "";
                }
                #endregion
            }
            else
            {
                UUIDText.text = $"UUID: ";
                positionText.text = $"Pos: ";
                scaleText.text = $"Scale: ";
                angleText.text = $"Angle: ";
                dropdownMenu.ItemsString = " ";
                componentDataText.text = "";
            }
        }

        /// <summary>
        /// Displays the debug menu.
        /// </summary>
        public override void Update()
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
                // Get the mouse position
                Vector2 mousePosition = Raylib.GetMousePosition();
                if (mousePosition.X < dividingLineX) return;
                SelectedEntity = null;
                // Iterate through entities to check for collision with the mouse position
                // List<Entity> collidedEntities = new();
                foreach (Entity entity in Entity.GetAllEntities())
                {
                    BoxCollider2D collider = entity.GetComponent<BoxCollider2D>();
                    if (collider != null)
                    {
                        if (collider.IsCollidingWith(cursor.GetComponent<BoxCollider2D>()))
                        {
                            // Store the selected entity
                            SelectedEntity = entity;
                            entityEnabledButton.isChecked = SelectedEntity.Enabled;
                            selectedEntityMarker.transform.Scale = SelectedEntity.transform.Scale;
                            selectedEntityMarker.transform.Position = SelectedEntity.transform.Position;
                            // collidedEntities.Add(collider.entity);
                            break; // Exit the loop after finding the first entity
                        }
                    }
                }
            }

            // Check if the left mouse button is being held down and a draggable entity is selected
            if (Raylib.IsMouseButtonDown(Raylib.MOUSE_LEFT_BUTTON) && SelectedEntity != null)
            {
                if (Raylib.GetMousePosition().X < dividingLineX) return;
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
                            Vector2 mousePos = Raylib.GetMousePosition() - new Vector2(dividingLineX, 0);
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

        /// <summary>
        /// Handles updates in the editor.
        /// </summary>
        public static void HandleUpdate()
        {
            componentDataText.transform.Position = dropdownMenu.transform.Position + new Vector2(130, 0);
            if (SelectedEntity != null)
            {
                selectedEntityMarker.transform.Scale = SelectedEntity.transform.Scale;
                selectedEntityMarker.transform.Position = SelectedEntity.transform.Position;
            }
        }

        /// <summary>
        /// Keeps the mouse in a valid position in the screen.
        /// </summary>
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

        public override void OnStop()
        {
            // throw new NotImplementedException();
        }

        /// <summary>
        /// Enumeration representing the state of the editor.
        /// </summary>
        public enum EditorState
        {
            /// <summary>
            /// Position state.
            /// </summary>
            Pos,

            /// <summary>
            /// Scale state.
            /// </summary>
            Scale,

            /// <summary>
            /// Rotation state.
            /// </summary>
            Rotation
        }
    }
}
