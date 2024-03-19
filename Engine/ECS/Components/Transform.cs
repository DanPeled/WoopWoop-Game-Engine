using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Raylib_cs;

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
            AddChild(child.ID);
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

        public override void OnDrawGizmo()
        {
            // Calculate the end points of the lines based on the angle and position
            Vector2 horizontalEnd = position + Vector2.Transform(new Vector2(30, 0), Matrix3x2.CreateRotation(MathUtil.DegToRad(angle)));
            Vector2 verticalEnd = position + Vector2.Transform(new Vector2(0, 30), Matrix3x2.CreateRotation(MathUtil.DegToRad(angle)));

            // Draw the horizontal line (with arrow)
            Raylib.DrawLineEx(position, horizontalEnd, 10, Color.Red);
            DrawArrow(position, horizontalEnd, 10, Color.Red);

            // Draw the vertical line (with arrow)
            Raylib.DrawLineEx(position, verticalEnd, 10, Color.Green);
            DrawArrow(position, verticalEnd, 10, Color.Green);
        }


        private void DrawArrow(Vector2 start, Vector2 end, float size, Color color)
        {
            // Calculate the direction of the line
            Vector2 direction = Vector2.Normalize(end - start);

            // Calculate the perpendicular vector (to get the arrow's width)
            Vector2 perpendicular = new Vector2(-direction.Y, direction.X);

            // Normalize the perpendicular vector and scale it to adjust the arrowhead width
            perpendicular = Vector2.Normalize(perpendicular);
            perpendicular *= size / 4; // Adjust this factor to control the arrowhead width

            // Rotate the arrow points based on the angle
            Matrix3x2 rotationMatrix = Matrix3x2.CreateRotation(MathUtil.DegToRad(entity.transform.Angle));
            Vector2 rotatedPerpendicular = Vector2.Transform(perpendicular, rotationMatrix);

            // Calculate points for arrowhead
            Vector2 arrowPoint1 = end - (direction * size) + rotatedPerpendicular;
            Vector2 arrowPoint2 = end - (direction * size) - rotatedPerpendicular;

            // Draw arrowhead
            Raylib.DrawLineEx(end, arrowPoint1, size, color);
            Raylib.DrawLineEx(end, arrowPoint2, size, color);
        }



    }
}
