using System.Numerics;

namespace WoopWoopEngine.UI
{
    /// <summary>
    /// Represents a horizontal grid component that arranges its child elements horizontally.
    /// </summary>
    public class HorizontalGrid : Component
    {
        /// <summary>
        /// The spacing between child elements.
        /// </summary>
        public float spacing = 10;

        /// <summary>
        /// The additional spacing to the left of the first child element.
        /// </summary>
        public float spacingLeft = 5;

        /// <summary>
        /// Updates the position of child elements based on the horizontal grid layout.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last update.</param>
        public override void Update(float deltaTime)
        {
            Transform[] children = transform.GetChildren();
            for (int i = 0; i < children.Length; i++)
            {
                float additionalSpacing = spacingLeft;
                if (i > 0)
                {
                    additionalSpacing = children[i - 1].transform.Scale.X;
                }
                children[i].transform.Position = new Vector2(transform.Position.X + spacing * i + additionalSpacing, children[i].transform.Position.Y);
                Entity.GetEntityWithUUID(children[i].entity.ID).transform = children[i].transform;
            }
        }
    }
}
