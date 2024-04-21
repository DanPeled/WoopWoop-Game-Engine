using ZeroElectric.Vinculum;
using System.Numerics;

namespace WoopWoopEngine
{
    /// <summary>
    /// Base class for rendering components.
    /// </summary>
    [RequireComponent(typeof(Transform))]
    public class Renderer : Component
    {
        /// <summary>
        /// The color of the renderer.
        /// </summary>
        public Color Color = Raylib.BLACK;

        private byte layer = 0;

        /// <summary>
        /// The rendering layer of the renderer.
        /// </summary>
        public byte Layer
        {
            get { return layer; }
            set
            {
                layer = value;
                WoopWoop.ChangeRenderLayer(this);
            }
        }

        /// <summary>
        /// Gets the original position of the transform based on the pivot.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="pivot">The pivot point.</param>
        /// <returns>The original position of the transform.</returns>
        protected static Vector2 GetOriginalPositionOfTransform(Transform transform, Pivot pivot)
        {
            Vector2 value = transform.Position;
            switch (pivot)
            {
                case Pivot.Center:
                    {
                        value += transform.Scale / 2;
                        break;
                    }
                case Pivot.BottomRight:
                    {
                        value += transform.Scale;
                        break;
                    }
                case Pivot.TopRight:
                    {
                        value.X += transform.Scale.X;
                        break;
                    }
                case Pivot.BottomLeft:
                    {
                        value.Y += transform.Scale.Y;
                        break;
                    }
                default:
                    break;
            }
            return value;
        }
    }
}
