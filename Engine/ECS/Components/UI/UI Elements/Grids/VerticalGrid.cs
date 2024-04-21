using System.Numerics;

namespace WoopWoopEngine.UI
{
    /// <summary>
    /// Represents a component that arranges child entities in a vertical grid layout.
    /// </summary>
    public class VerticalGrid : Component
    {
        /// <summary>
        /// The spacing between each child entity in the grid.
        /// </summary>
        public float Spacing { get; set; } = 10;

        /// <summary>
        /// The additional spacing applied to the top of the grid.
        /// </summary>
        public float SpacingTop { get; set; } = 5;

        /// <summary>
        /// Updates the component's state.
        /// </summary>
        /// <param name="deltaTime">The time passed since the last frame.</param>
        public override void Update(float deltaTime)
        {
            Transform[] children = transform.GetChildren();
            for (int i = 0; i < children.Length; i++)
            {
                float additionalSpacing = SpacingTop;
                if (i > 0)
                {
                    additionalSpacing = children[i - 1].transform.Scale.Y;
                }
                children[i].transform.Position = new Vector2(
                    children[i].transform.Position.X,
                    transform.Position.Y + Spacing * i + additionalSpacing
                );
                Entity.GetEntityWithUUID(children[i].entity.ID).transform = children[i].transform;
            }
        }
    }
}
