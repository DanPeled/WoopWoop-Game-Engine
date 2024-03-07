using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WoopWoop
{
    /// <summary>
    /// Represents an entity in the game world.
    /// </summary>
    public class Entity
    {
        private List<Component> components; // List of components attached to the entity
        public Transform transform; // The transform component of the entity
        private readonly object componentsLock = new object(); // Lock object for synchronizing access to the components list

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        public Entity()
        {
            components = new List<Component>();
            transform = AddComponent<Transform>();
        }

        /// <summary>
        /// Updates the entity.
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// Draws the entity.
        /// </summary>
        public virtual void Draw() { }

        /// <summary>
        /// Adds a new component of type T to the entity.
        /// </summary>
        /// <typeparam name="T">The type of component to add.</typeparam>
        /// <returns>The added component.</returns>
        public T AddComponent<T>() where T : Component, new()
        {
            T comp = new T();
            comp.Attach(this);
            lock (componentsLock)
            {
                components.Add(comp);
            }
            return GetComponent<T>();
        }

        /// <summary>
        /// Gets the component of type T attached to the entity.
        /// </summary>
        /// <typeparam name="T">The type of component to get.</typeparam>
        /// <returns>The component of type T, or null if not found.</returns>
        public T GetComponent<T>() where T : Component
        {
            lock (componentsLock)
            {
                return components.Find(c => c.GetType() == typeof(T)) as T;
            }
        }

        /// <summary>
        /// Performs internal updates on all components attached to the entity.
        /// </summary>
        public void InternalUpdate()
        {
            // Parallelize component updates
            Parallel.ForEach(GetComponents(), c =>
            {
                c.Update();
            });
        }

        /// <summary>
        /// Gets an array of all components attached to the entity.
        /// </summary>
        /// <returns>An array of components.</returns>
        public Component[] GetComponents()
        {
            lock (componentsLock)
            {
                return components.ToArray();
            }
        }
    }
}
