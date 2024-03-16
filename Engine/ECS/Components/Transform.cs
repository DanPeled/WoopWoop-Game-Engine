using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace WoopWoop
{
    public class Transform : Component
    {
        private Vector2 position = Vector2.Zero;
        private List<string> childrenUUIDs = new();

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
        public float Angle
        {
            get { return angle; }
            set
            {
                angle = (value % 360 + 360) % 360;
            }
        }

        public void AddChild(string childUUID)
        {
            childrenUUIDs.Add(childUUID);
        }

        public void AddChild(Entity child)
        {
            AddChild(child.UUID);
        }

        public Transform[] GetChildren()
        {
            return childrenUUIDs.Select(WoopWoopEngine.GetEntityWithUUID).Select(e => e.transform).ToArray();
        }

        public int GetChildCount()
        {
            return childrenUUIDs.Count;
        }
    }
}
