using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
            BasicShapeRenderer renderer = new BasicShapeRenderer();
            renderer.shape = BasicShape.Box;
            renderer.Color = new Color(130, 130, 130, 200);
            renderer.Layer = 255;
            editorWindow.AddComponent(renderer);
            editorWindow.transform.Scale = new(45, 50);
            editorWindow.transform.Position = new(WoopWoopEngine.screenWidth - (editorWindow.transform.Scale.X * 10 - 5), 10);


            Entity uuidTextEntity = new();
            UUIDText = uuidTextEntity.AddComponent<TextRenderer>();
            uuidTextEntity.transform.Position = new(WoopWoopEngine.screenWidth - (editorWindow.transform.Scale.X * 10 - 17), 12);
            UUIDText.text = "UUID: ";

            Entity positionTextEntity = new();
            positionText = positionTextEntity.AddComponent<TextRenderer>();
            positionTextEntity.transform.Position = new(WoopWoopEngine.screenWidth - (editorWindow.transform.Scale.X * 10 - 17), 26);
            positionText.text = "Pos: ";
            positionText.Color = Color.Black;

            Entity scaleTextEntity = new();
            scaleText = scaleTextEntity.AddComponent<TextRenderer>();
            scaleTextEntity.transform.Position = new(WoopWoopEngine.screenWidth - (editorWindow.transform.Scale.X * 10 - 17), 70);
            scaleText.text = "Scale: ";
            scaleText.Color = Color.Black;


            Entity angleTextEntity = new();
            angleText = angleTextEntity.AddComponent<TextRenderer>();
            angleTextEntity.transform.Position = new(WoopWoopEngine.screenWidth - (editorWindow.transform.Scale.X * 10 - 17), 130);
            angleText.text = "Angle: ";
            angleText.Color = Color.Black;

            Entity compoenetsTextEntity = new();
            componentsText = compoenetsTextEntity.AddComponent<TextRenderer>();
            compoenetsTextEntity.transform.Position = new(WoopWoopEngine.screenWidth - (editorWindow.transform.Scale.X * 10 - 17), 170);
            componentsText.text = "Components: ";
            componentsText.Color = Color.Black;

            editorWindow.Enabled = false;



            editorWindow.RemoveComponent<PointerCollider>();
            WoopWoopEngine.Instantiate(editorWindow);
            WoopWoopEngine.Instantiate(uuidTextEntity);
            WoopWoopEngine.Instantiate(positionTextEntity);
            WoopWoopEngine.Instantiate(compoenetsTextEntity);
            WoopWoopEngine.Instantiate(angleTextEntity);
            WoopWoopEngine.Instantiate(scaleTextEntity);
            editorWindow.transform.AddChild(uuidTextEntity);
            editorWindow.transform.AddChild(positionTextEntity);
            editorWindow.transform.AddChild(angleTextEntity);
            editorWindow.transform.AddChild(scaleTextEntity);
            editorWindow.transform.AddChild(compoenetsTextEntity);


            List<Entity> entities = new List<Entity>() { editorWindow };
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

                    if (component.GetType() != typeof(PointerCollider))
                        componentsString += component.GetType().Name + ", ";
                }
                componentsText.text = $"Components: {componentsString}" + " " + selectedEntity.Name;
            }
        }
    }

}
