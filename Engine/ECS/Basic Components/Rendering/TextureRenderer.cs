using Raylib_cs;

namespace WoopWoop
{
    public class TextureRenderer : Renderer
    {
        public Texture2D texture;
        public void LoadImage(Image image)
        {
            texture = Raylib.LoadTextureFromImage(image);
        }

        public void LoadFromPath(string filename)
        {
            Image image = Raylib.LoadImage(filename);
            LoadImage(image);
            Raylib.UnloadImage(image);
        }

        public override void Update(float deltaTime)
        {
            Raylib.DrawTextureEx(texture, entity.transform.Position, entity.transform.Angle, entity.transform.Scale.X, color);
        }

        public override void Stop()
        {
            Raylib.UnloadTexture(texture);
        }
    }
}