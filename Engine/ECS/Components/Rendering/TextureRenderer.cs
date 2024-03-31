using System.Numerics;
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
                Debug.WriteWarning($"No texture provided, nothing will be rendered on entity {entity.ID}");
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
                // Separate X and Y scaling factors
                Vector2 scale = new(transform.Scale.X, transform.Scale.Y);

                // Calculate adjusted position to maintain apparent distance
                Vector2 adjustedPosition = new(
                    transform.Position.X - (texture.Width * scale.X - texture.Width) / 2,
                    transform.Position.Y - (texture.Height * scale.Y - texture.Height) / 2
                );


                // Adjust the width based on X scaling factor to prevent diagonal movement
                float adjustedWidth = texture.Width * scale.X;
                float adjustedHeight = texture.Height * scale.Y;

                // Draw the texture with separate scaling factors for width and height
                Raylib.DrawTexturePro(
                    texture,
                    new Rectangle(0, 0, texture.Width, texture.Height), // Source rectangle (entire texture)
                    new Rectangle(adjustedPosition.X, adjustedPosition.Y, adjustedWidth, adjustedHeight), // Destination rectangle
                    new Vector2(adjustedWidth / 2, adjustedHeight / 2), // Origin (center of the scaled texture)
                    transform.Angle,
                    Color
                );
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
