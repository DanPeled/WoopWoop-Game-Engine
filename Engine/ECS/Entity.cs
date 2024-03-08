using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace WoopWoop
{
    /// <summary>
    /// Represents an entity in the game world.
    /// </summary>
    public class Entity
    {
        //TODO: Add children and parent relations
        private List<Component> components; // List of components attached to the entity
        public Transform transform; // The transform component of the entity
        private readonly object componentsLock = new object(); // Lock object for synchronizing access to the components list
        private bool enabled = true;
        Guid uuid;
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                //TODO: Enable / disable all children
                enabled = value;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        public Entity()
        {
            uuid = Guid.NewGuid();
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
            return comp;
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
        /// Removes the component of the specified type from the entity.
        /// </summary>
        /// <typeparam name="T">The type of component to remove.</typeparam>
        public void RemoveComponent<T>() where T : Component
        {
            lock (componentsLock)
            {
                var componentToRemove = components.Find(c => c.GetType() == typeof(T));
                if (componentToRemove != null)
                {
                    components.Remove(componentToRemove);
                }
            }
        }
        /// <summary>
        /// Performs internal updates on all components attached to the entity.
        /// </summary>
        public void InternalUpdate()
        {
            // Check if all required components for each component are present
            foreach (var component in GetComponents())
            {
                // Check if the component has the RequireComponent attribute
                var requireComponentAttribute = component.GetType().GetCustomAttribute<RequireComponent>();
                if (requireComponentAttribute != null)
                {
                    // Get the required component types
                    var requiredComponents = requireComponentAttribute.requiredComponents;

                    // Check if all required components are attached to the entity
                    if (requiredComponents.All(reqType => HasComponentOfType(reqType)))
                    {
                        // Proceed with updating the component
                        if (Enabled && component.Enabled)
                        {
                            component.Update();
                        }
                    }
                    else
                    {
                        // Handle case where the component is missing
                        var missingComponents = string.Join(", ", requiredComponents
                            .Where(reqType => !HasComponentOfType(reqType))
                            .Select(reqType => reqType.Name));
                        Debug.WriteError(
                            $"Cannot update component {component.GetType().Name} because required components ({missingComponents}) are missing.");
                    }
                }
                else
                {
                    // No required components specified, proceed with updating the component
                    if (Enabled && component.Enabled)
                    {
                        component.Update();
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the entity has a component of the specified type attached.
        /// </summary>
        /// <param name="componentType">The type of component to check for.</param>
        /// <returns>True if the entity has a component of the specified type, otherwise false.</returns>
        private bool HasComponentOfType(Type componentType)
        {
            lock (componentsLock)
            {
                return components.Any(c => c.GetType() == componentType);
            }
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
