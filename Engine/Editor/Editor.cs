using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raylib_cs;
using WoopWoop.UI;

namespace WoopWoop
{
    namespace Editor
    {
        public static class Editor
        {
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
            private static TextRenderer UUIDText, positionText;
            public static void Init()
            {
                BasicShapeRenderer renderer = new BasicShapeRenderer();
                renderer.shape = BasicShape.Box;
                renderer.Color = new Color(130, 130, 130, 200);
                renderer.Layer = 255;
                editorWindow.AddComponent(renderer);
                editorWindow.transform.Scale = new(45, 50);
                editorWindow.transform.Position = new(WoopWoopEngine.width - (editorWindow.transform.Scale.X * 10 - 5), 10);


                Entity uuidTextEntity = new();
                UUIDText = uuidTextEntity.AddComponent<TextRenderer>();
                uuidTextEntity.transform.Position = new(WoopWoopEngine.width - (editorWindow.transform.Scale.X * 10 - 17), 12);
                UUIDText.text = "UUID: ";

                Entity positionTextEntity = new();
                positionText = positionTextEntity.AddComponent<TextRenderer>();
                positionTextEntity.transform.Position = new(WoopWoopEngine.width - (editorWindow.transform.Scale.X * 10 - 17), 26);
                positionText.text = "Pos: ";

                editorWindow.Enabled = false;
                editorWindow.RemoveComponent<PointerCollider>();
                WoopWoopEngine.Instantiate(uuidTextEntity);
                WoopWoopEngine.Instantiate(positionTextEntity);
                debugMenuEntities.Add(editorWindow);
                debugMenuEntities.Add(positionTextEntity);
                debugMenuEntities.Add(uuidTextEntity);
                WoopWoopEngine.Instantiate(editorWindow);
                editorWindow.transform.AddChild(uuidTextEntity);
                editorWindow.transform.AddChild(positionTextEntity);
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
                    positionText.text = $"Pos: {selectedEntity.transform.Position}";
                }
            }
        }
    }
}