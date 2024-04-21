using System.Numerics;
using ZeroElectric.Vinculum;

namespace WoopWoopEngine
{
    /// <summary>
    /// Renders a texture on an entity.
    /// </summary>
    public class TextureRenderer : Renderer
    {
        /// <summary>
        /// The texture to render.
        /// </summary>
        public Texture texture;

        private bool isTextureLoaded = false;

        /// <summary>
        /// Called when the component starts.
        /// </summary>
        public override void Start()
        {
            if (!isTextureLoaded)
            {
                Debug.WriteWarning($"No texture provided, nothing will be rendered on entity {entity.ID}");
            }
        }

        /// <summary>
        /// Loads a texture from an image.
        /// </summary>
        /// <param name="image">The image to load the texture from.</param>
        public void LoadImage(Image image)
        {
            texture = Raylib.LoadTextureFromImage(image);
            isTextureLoaded = true;
        }

        /// <summary>
        /// Loads a texture from a file path.
        /// </summary>
        /// <param name="filename">The path to the image file.</param>
        public void LoadFromPath(string filename)
        {
            Image image = Raylib.LoadImage(filename);
            LoadImage(image);
            Raylib.UnloadImage(image);
        }

        /// <summary>
        /// Updates the texture renderer.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last update.</param>
        public override void Update(float deltaTime)
        {
            if (isTextureLoaded)
            {
                // Separate X and Y scaling factors
                Vector2 scale = new(transform.Scale.X, transform.Scale.Y);

                // Calculate adjusted position to maintain apparent distance
                Vector2 adjustedPosition = new(
                    transform.Position.X - (texture.width * scale.X - texture.width) / 2,
                    transform.Position.Y - (texture.height * scale.Y - texture.height) / 2
                );

                // Adjust the width based on X scaling factor to prevent diagonal movement
                float adjustedWidth = texture.width * scale.X;
                float adjustedHeight = texture.height * scale.Y;

                // Draw the texture with separate scaling factors for width and height
                Raylib.DrawTexturePro(
                    texture,
                    new Rectangle(0, 0, texture.width, texture.height), // Source rectangle (entire texture)
                    new Rectangle(adjustedPosition.X, adjustedPosition.Y, adjustedWidth, adjustedHeight), // Destination rectangle
                    new Vector2(adjustedWidth / 2, adjustedHeight / 2), // Origin (center of the scaled texture)
                    transform.Angle,
                    Color
                );
            }
        }

        /// <summary>
        /// Called when the component stops.
        /// </summary>
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
