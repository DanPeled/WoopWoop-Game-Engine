using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace WoopWoop
{
    /// <summary>
    /// Represents the transformation component of an entity.
    /// </summary>
    public class Transform : Component
    {
        private Vector2 position = Vector2.Zero;
        private List<string> childrenUUIDs = new();

        /// <summary>
        /// Gets or sets the position of the transform.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set
            {
                // Calculate the offset by which the parent's position has changed
                Vector2 offset = value - position;

                // Update the position of each child by the same offset
                foreach (Transform childTransform in GetChildren())
                {
                    childTransform.Position += offset;
                }

                // Update the parent's position
                position = value;
            }
        }

        private Vector2 scale = Vector2.One;
        /// <summary>
        /// Gets or sets the scale of the transform.
        /// </summary>
        public Vector2 Scale
        {
            get { return scale; }
            set
            {
                foreach (Transform childTransform in GetChildren())
                {
                    childTransform.Scale = value;
                }
                scale = value;
            }
        }

        private float angle = 0;
        /// <summary>
        /// Gets or sets the rotation angle (in degrees) of the transform.
        /// </summary>
        public float Angle
        {
            get { return angle; }
            set
            {
                angle = (value % 360 + 360) % 360;
            }
        }

        /// <summary>
        /// Adds a child entity to this transform.
        /// </summary>
        /// <param name="childUUID">The UUID of the child entity.</param>
        public void AddChild(string childUUID)
        {
            childrenUUIDs.Add(childUUID);
        }

        /// <summary>
        /// Adds a child entity to this transform.
        /// </summary>
        /// <param name="child">The child entity to add.</param>
        public void AddChild(Entity child)
        {
            AddChild(child.UUID);
        }

        /// <summary>
        /// Gets an array of child transforms.
        /// </summary>
        /// <returns>An array of child transforms.</returns>
        public Transform[] GetChildren()
        {
            return childrenUUIDs.Select(WoopWoopEngine.GetEntityWithUUID).Select(e => e.transform).ToArray();
        }

        /// <summary>
        /// Gets the count of child entities.
        /// </summary>
        /// <returns>The count of child entities.</returns>
        public int GetChildCount()
        {
            return childrenUUIDs.Count;
        }
    }
}
