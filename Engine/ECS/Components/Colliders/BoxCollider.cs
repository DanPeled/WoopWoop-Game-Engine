using System;
using System.Collections.Generic;
using System.Numerics;
using ZeroElectric.Vinculum;

namespace WoopWoop
{
    public class BoxCollider : Collider
    {
        // We'll keep track of the axes of the collider
        private List<Vector2> axes = new List<Vector2>();

        public override void Awake()
        {
            base.Awake();
        }

        public override bool IsCollidingWith(Collider other)
        {
            if (other is BoxCollider)
            {
                other = (BoxCollider)other;
                // Get the transform component of both entities
                Transform otherTransform = other.transform;
                Vector2 offset = new(0, 0);
                if (other.entity == Editor.Editor.cursor && WoopWoopEngine.IsInDebugMenu)
                {
                    offset = new(Editor.Editor.dividingLineX, 0);
                }
                // Get the corner points of both colliders in world space
                List<Vector2> thisPoints = GetWorldPoints(transform.Position, transform.Scale, transform.Angle);
                List<Vector2> otherPoints = GetWorldPoints(otherTransform.Position - offset, otherTransform.Scale, otherTransform.Angle);

                // Check for collision using SAT
                return SATCollisionDetection(thisPoints, otherPoints);
            }
            return false;
        }

        private List<Vector2> GetWorldPoints(Vector2 position, Vector2 scale, float angle)
        {
            // Calculate the corner points of the collider in local space
            List<Vector2> localPoints = new()
    {
        new Vector2(-scale.X / 2, -scale.Y / 2),
        new Vector2(scale.X / 2, -scale.Y / 2),
        new Vector2(scale.X / 2, scale.Y / 2),
        new Vector2(-scale.X / 2, scale.Y / 2)
    };

            // Rotate the points
            List<Vector2> rotatedPoints = new List<Vector2>();
            foreach (var point in localPoints)
            {
                // Apply rotation
                float rotatedX = point.X * (float)Math.Cos(angle) - point.Y * (float)Math.Sin(angle);
                float rotatedY = point.X * (float)Math.Sin(angle) + point.Y * (float)Math.Cos(angle);

                // Translate to world space
                rotatedPoints.Add(new Vector2(rotatedX, rotatedY) + position);
            }

            return rotatedPoints;
        }


        private bool SATCollisionDetection(List<Vector2> points1, List<Vector2> points2)
        {
            // Combine the axes of both colliders
            axes.Clear();
            axes.AddRange(GetAxes(points1));
            axes.AddRange(GetAxes(points2));

            // Project the points of both colliders onto each axis
            foreach (var axis in axes)
            {
                float min1 = float.MaxValue;
                float max1 = float.MinValue;
                float min2 = float.MaxValue;
                float max2 = float.MinValue;

                foreach (var point in points1)
                {
                    float projection = Vector2.Dot(point, axis);
                    min1 = Math.Min(min1, projection);
                    max1 = Math.Max(max1, projection);
                }

                foreach (var point in points2)
                {
                    float projection = Vector2.Dot(point, axis);
                    min2 = Math.Min(min2, projection);
                    max2 = Math.Max(max2, projection);
                }

                // Check for overlap
                if (!(max1 >= min2 && max2 >= min1))
                {
                    // No overlap found, hence there's no collision
                    return false;
                }
            }

            // If all axes have overlapping projections, then the colliders are intersecting
            return true;
        }

        private List<Vector2> GetAxes(List<Vector2> points)
        {
            // Calculate the axes perpendicular to the edges of the collider
            List<Vector2> axes = new List<Vector2>();
            for (int i = 0; i < points.Count; i++)
            {
                Vector2 p1 = points[i];
                Vector2 p2 = points[(i + 1) % points.Count];

                Vector2 edge = p2 - p1;
                Vector2 normal = new Vector2(-edge.Y, edge.X); // Perpendicular to edge
                normal = Vector2.Normalize(normal);
                axes.Add(normal);
            }
            return axes;
        }
        public Vector2 GetWorldExtents()
        {
            // Get the scale of the transform
            Vector2 scale = transform.Scale;

            // Calculate the half-width and half-height of the collider in world space
            float halfWidth = scale.X / 2;
            float halfHeight = scale.Y / 2;

            // Calculate the extents vector
            return new Vector2(halfWidth, halfHeight);
        }
        public override void OnDrawGizmo()
        {
            // Get the corner points of the collider in world space
            List<Vector2> points = GetWorldPoints(transform.Position, transform.Scale, MathUtil.DegToRad(transform.Angle));

            // Calculate the corner points of the rectangle
            Vector2 topLeft = points[0];
            Vector2 topRight = points[1];
            Vector2 bottomRight = points[2];
            Vector2 bottomLeft = points[3];

            // Draw the rectangle lines
            Raylib.DrawLineEx(topLeft, topRight, 2, Raylib.RED);
            Raylib.DrawLineEx(topRight, bottomRight, 2, Raylib.RED);
            Raylib.DrawLineEx(bottomRight, bottomLeft, 2, Raylib.RED);
            Raylib.DrawLineEx(bottomLeft, topLeft, 2, Raylib.RED);
        }

    }
}
