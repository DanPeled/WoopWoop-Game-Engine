using System.Numerics;

namespace WoopWoop
{
    /// <summary>
    /// Represents the transformation component of an entity.
    /// </summary>
    public class Transform : Component
    {
        /// <summary>
        /// The pivot point of the transform.
        /// </summary>
        public Pivot pivot = Pivot.Center;

        /// <summary>
        /// The position of the transform.
        /// </summary>
        private Vector2 position = Vector2.Zero;

        /// <summary>
        /// The UUIDs of child entities.
        /// </summary>
        private List<string> childrenUUIDs = new List<string>();

        /// <summary>
        /// The parent transform of this transform.
        /// </summary>
        public Transform parent { get; private set; }

        /// <summary>
        /// Event invoked when the transform is changed.
        /// </summary>
        public Action? onTransformChanged;

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

                position = value;
                onTransformChanged?.Invoke();
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
                // Calculate the scale factor relative to the current scale
                Vector2 scaleFactor = value / scale;

                // Apply the scale factor to the position of each child
                foreach (Transform childTransform in GetChildren())
                {
                    // Adjust the child position relative to the parent's scale
                    childTransform.Position = position + Vector2.Transform(childTransform.Position - position, Matrix3x2.CreateScale(scaleFactor));

                    // Scale the child's scale relative to the parent's scale
                    childTransform.Scale *= scaleFactor;
                }

                scale = value;
                onTransformChanged?.Invoke();
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
                // Calculate the angle difference
                float angleDifference = value - angle;

                // Update the angle of each child relative to the parent's position
                foreach (Transform childTransform in GetChildren())
                {
                    // Calculate the child's position relative to the parent
                    Vector2 relativePosition = childTransform.Position - position;

                    // Rotate the relative position by the angle difference
                    Matrix3x2 rotationMatrix = Matrix3x2.CreateRotation(MathUtil.DegToRad(angleDifference));
                    Vector2 rotatedRelativePosition = Vector2.Transform(relativePosition, rotationMatrix);

                    // Update the child's position with the rotated relative position
                    childTransform.Position = position + rotatedRelativePosition;
                }

                // Update the parent's angle
                angle = (value % 360 + 360) % 360;
                onTransformChanged?.Invoke();
            }
        }

        /// <summary>
        /// Adds a child entity to this transform.
        /// </summary>
        /// <param name="childUUID">The UUID of the child entity.</param>
        public void AddChild(string childUUID)
        {
            childrenUUIDs.Add(childUUID);
            Entity.GetEntityWithUUID(childUUID).transform.parent = this;
        }

        /// <summary>
        /// Adds a child entity to this transform.
        /// </summary>
        /// <param name="child">The child entity to add.</param>
        public void AddChild(Entity child)
        {
            AddChild(child.ID);
        }

        /// <summary>
        /// Adds a child entity to this transform.
        /// </summary>
        /// <param name="child">The child entity transform to add.</param>
        public void AddChild(Transform childTransform)
        {
            AddChild(childTransform.entity.ID);
        }

        /// <summary>
        /// Gets an array of child transforms.
        /// </summary>
        /// <returns>An array of child transforms.</returns>
        public Transform[] GetChildren()
        {
            if (childrenUUIDs != null)
                return childrenUUIDs.Select(Entity.GetEntityWithUUID)?.Select(e => e?.transform).ToArray();
            return null;
        }

        /// <summary>
        /// Gets the count of child entities.
        /// </summary>
        /// <returns>The count of child entities.</returns>
        public int GetChildCount()
        {
            return childrenUUIDs.Count;
        }

        /// <summary>
        /// Calculates the pivot-point offset of the transform.
        /// </summary>
        /// <returns>The calculated pivot-point offset.</returns>
        public Vector2 GetPivotPointOffset()
        {
            Vector2 value = Vector2.Zero;
            // Update the parent's position
            switch (pivot)
            {
                case Pivot.Center:
                    {
                        value = -Scale / 2;
                        break;
                    }
                case Pivot.BottomRight:
                    {
                        value = -Scale;
                        break;
                    }
                case Pivot.TopRight:
                    {
                        value.X = -Scale.X;
                        break;
                    }
                case Pivot.BottomLeft:
                    {
                        value.Y = -Scale.Y;
                        break;
                    }
                default:
                    break;
            }
            return value;
        }
    }

    /// <summary>
    /// The pivot points for positioning.
    /// </summary>
    public enum Pivot
    {
        TopLeft, TopRight, Center, BottomLeft, BottomRight
    }
}
