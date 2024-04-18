using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace WoopWoop
{
    /// <summary>
    /// Represents a collider component attached to an entity.
    /// </summary>
    [RequireComponent(typeof(Transform))]
    public class Collider : Component
    {
        /// <summary>
        /// Checks if the collider is colliding with another collider.
        /// </summary>
        /// <param name="other">The other collider to check against.</param>
        /// <returns>True if colliding, otherwise false.</returns>
        public virtual bool IsCollidingWith(Collider other) { return false; }

        /// <summary>
        /// Checks if the collider is colliding with an entity.
        /// </summary>
        /// <param name="other">The other entity to check against.</param>
        /// <returns>True if colliding, otherwise false.</returns>
        public bool IsCollidingWith(Entity other)
        {
            if (other == null || other == this.entity) return false;
            return IsCollidingWith(other.GetComponent<BoxCollider>());
        }

        /// <summary>
        /// Retrieves entities colliding with a list of entities.
        /// </summary>
        /// <param name="entitiesList">The list of entities to check against.</param>
        /// <returns>Array of colliding entities.</returns>
        private Entity[] GetCollidingEntitiesFromList(List<Entity> entitiesList)
        {
            List<Entity> collidingEntities = new();
            entitiesList.Remove(entity);
            foreach (Entity e in entitiesList)
            {
                if (IsCollidingWith(e))
                {
                    collidingEntities.Add(e);
                }
            }
            return collidingEntities.ToArray();
        }

        /// <summary>
        /// Gets an array of entities colliding with entities having a specific tag.
        /// </summary>
        /// <param name="tag">The tag to filter entities.</param>
        /// <returns>An array of colliding entities.</returns>
        public Entity[] GetCollidingEntitiesWithTag(string tag)
        {
            return GetCollidingEntitiesFromList(Entity.GetEntitiesWithTag(tag).ToList());
        }

        /// <summary>
        /// Gets an array of all entities colliding with the collider's entity.
        /// </summary>
        /// <returns>An array of colliding entities.</returns>
        public Entity[] GetAllCollidingEntities()
        {
            return GetCollidingEntitiesFromList(Entity.GetAllEntities().ToList());
        }
    }

    /// <summary>
    /// Represents collision data between two colliders.
    /// </summary>
    public struct CollisionData
    {
        /// <summary>
        /// The collider involved in the collision.
        /// </summary>
        public Collider collider;

        /// <summary>
        /// The number of contact points in the collision.
        /// </summary>
        public int contactCount;

        /// <summary>
        /// The contact points in the collision.
        /// </summary>
        public Vector2[] contactPoints;

        /// <summary>
        /// The entity involved in the collision.
        /// </summary>
        public Entity entity;

        /// <summary>
        /// The transform of the collider involved in the collision.
        /// </summary>
        public Transform transform;
    }
}
