using System;
using System.Collections.Generic;
using System.Numerics;
using WoopWoopEditor;
using ZeroElectric.Vinculum;

namespace WoopWoopEngine
{
    /// <summary>
    /// Represents a collider component for a rectangular box.
    /// </summary>
    public class BoxCollider2D : Collider
    {
        // We'll keep track of the axes of the collider
        private List<Vector2> axes = new List<Vector2>();

        /// <summary>
        /// Initializes the collider.
        /// </summary>
        public override void Awake()
        {
            base.Awake();
        }

        /// <summary>
        /// Checks if the collider is colliding with another collider.
        /// </summary>
        /// <param name="other">The other collider to check against.</param>
        /// <returns>True if colliding, otherwise false.</returns>
        public override bool IsCollidingWith(Collider other)
        {
            if (other is BoxCollider2D)
            {
                other = (BoxCollider2D)other;
                // Get the transform component of both entities
                Transform otherTransform = other.transform;
                Vector2 offset = new(0, 0);
                if (other.entity == Editor.cursor && WoopWoop.IsInDebugMenu)
                {
                    offset = new(Editor.dividingLineX, 0);
                }
                // Get the corner points of both colliders in world space
                List<Vector2> thisPoints = GetWorldPoints(transform.Position, transform.Scale, transform.Angle);
                List<Vector2> otherPoints = GetWorldPoints(otherTransform.Position - offset, otherTransform.Scale, otherTransform.Angle);

                // Check for collision using SAT
                return SATCollisionDetection(thisPoints, otherPoints);
            }
            return false;
        }

        /// <summary>
        /// Gets the corner points of the collider in world space.
        /// </summary>
        /// <param name="position">The position of the collider.</param>
        /// <param name="scale">The scale of the collider.</param>
        /// <param name="angle">The angle of rotation of the collider (in radians).</param>
        /// <returns>List of corner points in world space.</returns>
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

        /// <summary>
        /// Performs Separating Axis Theorem (SAT) collision detection.
        /// </summary>
        /// <param name="points1">Corner points of the first collider.</param>
        /// <param name="points2">Corner points of the second collider.</param>
        /// <returns>True if colliding, otherwise false.</returns>
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

        /// <summary>
        /// Calculates the axes of the collider.
        /// </summary>
        /// <param name="points">Corner points of the collider.</param>
        /// <returns>List of axes.</returns>
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

        /// <summary>
        /// Gets collision data with a given entity.
        /// </summary>
        /// <param name="other">The other collider to check against.</param>
        /// <returns>Collision data if colliding, otherwise null.</returns>
        public CollisionData? GetCollisionData(Collider other)
        {
            if (IsCollidingWith(other))
            {
                CollisionData collisionData = new CollisionData
                {
                    // Populate collision data
                    collider = other,
                    contactCount = 0,
                    contactPoints = new Vector3[0] // Initialize with empty array
                };

                // Get the corner points of both colliders in world space
                List<Vector2> thisPoints = GetWorldPoints(transform.Position, transform.Scale, transform.Angle);
                List<Vector2> otherPoints = GetWorldPoints(other.transform.Position, other.transform.Scale, other.transform.Angle);

                // Calculate the contact points using SAT
                List<Vector2> contactPoints = SATContactPoints(thisPoints, otherPoints);

                if (contactPoints.Count > 0)
                {
                    collisionData.contactCount = contactPoints.Count;
                    collisionData.contactPoints = contactPoints.Select(v => new Vector3(v.X, v.Y, 0f)).ToArray();
                }

                collisionData.entity = other.entity;
                collisionData.transform = other.transform;

                return collisionData;
            }
            else
            {
                return null; // No collision, return null
            }
        }

        /// <summary>
        /// Calculates the contact points using the Separating Axis Theorem (SAT).
        /// </summary>
        /// <param name="points1">The points of the first collider.</param>
        /// <param name="points2">The points of the second collider.</param>
        /// <returns>A list of contact points.</returns>
        private List<Vector2> SATContactPoints(List<Vector2> points1, List<Vector2> points2)
        {
            List<Vector2> contactPoints = new List<Vector2>();

            // Combine the axes of both colliders
            List<Vector2> axes = GetAxes(points1);
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
                    return contactPoints;
                }
            }

            // If all axes have overlapping projections, then the colliders are intersecting.
            // In this case, we can assume the contact points to be the corners of the overlapping area.
            contactPoints.AddRange(points1);
            contactPoints.AddRange(points2);

            return contactPoints;
        }

        /// <summary>
        /// Gets the extents of the collider in world space.
        /// </summary>
        /// <returns>Extents vector.</returns>
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

        /// <summary>
        /// Draws the collider's gizmo in the editor.
        /// </summary>
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
