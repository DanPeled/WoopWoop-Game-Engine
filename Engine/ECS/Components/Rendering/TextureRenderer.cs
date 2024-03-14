using Raylib_cs;

namespace WoopWoop
{
    public class TextureRenderer : Renderer
    {
        public Texture2D texture;
        private bool isTextureLoaded = false;

        public override void Start()
        {
            if (!isTextureLoaded)
            {
                Debug.WriteWarning($"No texture provided, nothing will be rendered on entity {entity.UUID}");
            }
        }

        public void LoadImage(Image image)
        {
            texture = Raylib.LoadTextureFromImage(image);
            isTextureLoaded = true;
        }

        public void LoadFromPath(string filename)
        {
            Image image = Raylib.LoadImage(filename);
            LoadImage(image);
            Raylib.UnloadImage(image);
        }

        public override void Update(float deltaTime)
        {
            if (isTextureLoaded)
            {
                Raylib.DrawTextureEx(texture, entity.transform.Position, entity.transform.Angle, entity.transform.Scale.X, color);
            }
        }

        public override void Stop()
        {
            if (isTextureLoaded)
            {
                Raylib.UnloadTexture(texture);
                isTextureLoaded = false;
            }
        }
    }
}
