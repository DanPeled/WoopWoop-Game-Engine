using System;
using System.Numerics;
using Raylib_cs;

namespace WoopWoop
{
    public class PointerCollider : Component
    {
        private Vector2 size = new(10, 10);
        public Vector2 Size
        {
            get { return size; }
            set
            {
                size = SetSize(value);
            }
        }
        public override void Update(float deltaTime)
        {
        }

        public bool CheckCollisionPointRec(Vector2 point)
        {
            size = SetSize(size);
            // Translate the point to local space relative to the rectangle
            Vector2 localPoint = point - entity.transform.Position;

            // Scale the local point by the entity's scale
            localPoint /= entity.transform.Scale;

            // Convert the angle from degrees to radians
            float angleRadians = MathUtil.DegToRad(entity.transform.Angle);

            // Rotate the local point by the entity's angle
            localPoint = RotateVector(localPoint, -angleRadians);

            // Calculate the half extents of the rectangle
            float halfWidth = Size.X / 2f;
            float halfHeight = Size.Y / 2f;

            // Calculate the distance from the point to the nearest edge of the rectangle
            float xDistance = Math.Abs(localPoint.X) - halfWidth;
            float yDistance = Math.Abs(localPoint.Y) - halfHeight;

            // If both distances are less than or equal to zero, the point is inside the rectangle
            return xDistance <= 0 && yDistance <= 0;
        }

        // Function to rotate a vector by an angle in radians
        private Vector2 RotateVector(Vector2 vector, float angle)
        {
            float cos = MathF.Cos(angle);
            float sin = MathF.Sin(angle);
            return new Vector2(
                vector.X * cos - vector.Y * sin,
                vector.X * sin + vector.Y * cos
            );
        }
        public override void OnDrawGizmo()
        {
            bool isSelected = Editor.Editor.SelectedEntity == entity;
            // Calculate the corner points of the rectangle in local space
            Vector2 topLeft = new Vector2(-Size.X / 2, -Size.Y / 2);
            Vector2 topRight = new Vector2(Size.X / 2, -Size.Y / 2);
            Vector2 bottomLeft = new Vector2(-Size.X / 2, Size.Y / 2);
            Vector2 bottomRight = new Vector2(Size.X / 2, Size.Y / 2);

            // Rotate the corner points based on the entity's angle (in radians)
            float angleRadians = MathUtil.DegToRad(entity.transform.Angle);
            topLeft = RotateVector(topLeft, angleRadians);
            topRight = RotateVector(topRight, angleRadians);
            bottomLeft = RotateVector(bottomLeft, angleRadians);
            bottomRight = RotateVector(bottomRight, angleRadians);

            // Translate the corner points to world space
            topLeft += entity.transform.Position;
            topRight += entity.transform.Position;
            bottomLeft += entity.transform.Position;
            bottomRight += entity.transform.Position;
            // Draw the rectangle outline
            Color color = new Color(0, 228, 48, 200);
            if (isSelected)
            {
                color = new Color(0, 228, 0, 255);
            }
            Raylib.DrawLineEx(topLeft, topRight, 5, color);
            Raylib.DrawLineEx(topRight, bottomRight, 5, color);
            Raylib.DrawLineEx(bottomRight, bottomLeft, 5, color);
            Raylib.DrawLineEx(bottomLeft, topLeft, 5, color);
        }
        private Vector2 SetSize(Vector2 newSize)
        {
            // Calculate the size with a constant offset based on the entity's scale
            Vector2 size = new Vector2(newSize.X * entity.transform.Scale.X, newSize.Y * entity.transform.Scale.Y);

            // Ensure that the size does not go below a certain threshold
            size.X = Math.Max(size.X, 10); // Minimum size in X direction
            size.Y = Math.Max(size.Y, 10); // Minimum size in Y direction

            return size;
        }


    }
}

