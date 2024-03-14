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
        private List<Component> components; // List of components attached to the entity
        public Transform transform; // The transform component of the entity
        private readonly object componentsLock = new object(); // Lock object for synchronizing access to the components list
        private bool enabled = true;
        public string UUID { get; private set; }

        private List<string> childrenUUIDs = new();
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                //Enable / disable all children
                childrenUUIDs.ForEach(uuid =>
                {
                    WoopWoopEngine.GetEntityWithUUID(uuid).Enabled = value;
                });

                enabled = value;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        public Entity()
        {
            UUID = Guid.NewGuid().ToString();
            components = new List<Component>();
            transform = AddComponent<Transform>();
        }

        /// <summary>
        /// Updates the entity.
        /// </summary>
        public virtual void Update(float deltaTime) { }

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
        public void InternalUpdate(float deltaTime)
        {
            if (!Enabled) return;
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
                        if (component.Enabled)
                        {
                            component.Update(deltaTime);
                        }
                    }
                    else
                    {
                        // Handle case where the component is missing
                        var missingComponents = string.Join(", ", requiredComponents
                            .Where(reqType => !HasComponentOfType(reqType))
                            .Select(reqType => reqType.Name));
                        if (requireComponentAttribute.messageType == MessageType.Error)
                        {
                            Debug.WriteError(
                                $"Cannot update component {component.GetType().Name} because required components ({missingComponents}) are missing.");
                            return;
                        }
                        else if (requireComponentAttribute.messageType == MessageType.Warning)
                        {
                            Debug.WriteWarning(
                                $"{component.GetType().Name} may not function properly because required components ({missingComponents}) are missing.");
                            return;
                        }
                    }
                }
                else
                {
                    // No required components specified, proceed with updating the component
                    if (component.Enabled)
                    {
                        component.Update(deltaTime);
                    }
                }
            }
            Update(deltaTime);
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

        public void AddChild(string childUUID)
        {
            childrenUUIDs.Add(childUUID);
        }

        public void AddChild(Entity child)
        {
            AddChild(child.UUID);
        }

        public Entity[] GetChildren()
        {
            return childrenUUIDs.Select(WoopWoopEngine.GetEntityWithUUID).ToArray();
        }

        public int GetChildCount()
        {
            return childrenUUIDs.Count;
        }
    }
}
