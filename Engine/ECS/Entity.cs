using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using System.Threading.Tasks;

namespace WoopWoop
{
    /// <summary>
    /// Represents an entity in the game world.
    /// </summary>
    public class Entity
    {
        private HashSet<Component> components; // List of components attached to the entity
        public Transform transform; // The transform component of the entity
        private readonly object componentsLock = new(); // Lock object for synchronizing access to the components list
        private bool enabled = true;
        public readonly string ID;
        public string tag = "";
        public string Name;
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                //Enable / disable all children
                transform.GetChildren().ToList().ForEach(e =>
                {
                    e.entity.Enabled = value;
                });

                enabled = value;

            }
        }
        private static List<Entity> entities = new();
        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        public Entity() : this(Vector2.Zero) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// <param name="startPos">The initial position of the entity</param>
        /// </summary>
        public Entity(Vector2 startPos)
        {

            ID = Guid.NewGuid().ToString();
            components = new();
            transform = AddComponent<Transform>();
            transform.Position = startPos;
            AddComponent<PointCollider>().Size = new(10, 10);
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
            T comp = new();
            return AddComponent(comp);
        }

        public T AddComponent<T>(T comp) where T : Component
        {
            comp.Attach(this);
            lock (componentsLock)
            {
                components.Add(comp);
                if (GetEntityWithUUID(ID) != null)
                {
                    comp.Start();
                }
                if (comp is Renderer)
                {
                    WoopWoopEngine.AddToRenderBatch((Renderer)(object)comp);
                }
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
                return components.FirstOrDefault(c => c is T) as T;
            }
        }

        /// <summary>
        /// Removes the component of the specified type from the entity.
        /// </summary>
        /// <typeparam name="T">The type of component to remove.</typeparam>
        public void RemoveComponent<T>() where T : Component
        {
            RemoveComponent(typeof(T));
        }

        /// <summary>
        /// Removes the component of the specified type from the entity.
        /// </summary>
        /// <param name="componentType">The type of component to remove.</param>

        public void RemoveComponent(Type componentType)
        {
            lock (componentsLock)
            {
                var componentToRemove = components.FirstOrDefault(c => c.GetType() == componentType);
                if (componentToRemove != null)
                {
                    componentToRemove.Stop();
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
                if (component is Renderer) continue;
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
        public bool HasComponentOfType(Type componentType)
        {
            if (!typeof(Component).IsAssignableFrom(componentType))
            {
                throw new ArgumentException("componentType must be a subtype of Component");
            }

            lock (componentsLock)
            {
                return components.Any(c => componentType.IsInstanceOfType(c));
            }
        }


        public bool HasComponentOfType<T>() where T : Component
        {
            lock (componentsLock)
            {
                return components.Any(c => c is T);
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

        /// <summary>
        /// Checks if the entity has a component of the specified type attached.
        /// </summary>
        /// <typeparam name="T">The type of component to check for.</typeparam>
        /// <returns>True if the entity has a component of the specified type, otherwise false.</returns>
        public bool HasComponent<T>() where T : Component
        {
            return components.FirstOrDefault(c => c is T) != null;
        }

        public static void Instantiate(Entity entity)
        {
            entities.Add(entity);
            foreach (var component in entity.GetComponents())
            {
                component.Awake();
                if (component.Enabled)
                {
                    component.Start();
                }
            }
        }
        public static Entity[] GetAllEntities()
        {
            return entities.ToArray() is null ? new Entity[0] : entities.ToArray();
        }

        public static Entity GetEntityWithUUID(string uuid)
        {
            return entities.Find(e => e.ID.Equals(uuid));
        }
        public static Entity[] GetEntitiesWithTag(string tag)
        {
            return entities.FindAll(e => e.tag.Equals(tag)).ToArray();
        }

        public static void Destroy(Entity entity)
        {

            var entitiesCopy = new List<Entity>(entities);
            if (entity.transform != null)
            {
                foreach (Transform child in entity.transform.GetChildren())
                {
                    if (child.entity != null)
                    {
                        StopComponents(child.entity);
                        entitiesCopy.Remove(child.entity);
                    }
                }
            }

            StopComponents(entity);
            entitiesCopy.Remove(entity);
            entities = entitiesCopy;
        }

        private static void StopComponents(Entity entity)
        {
            foreach (Component c in entity.GetComponents())
            {
                entity.RemoveComponent(c.GetType());
            }
        }
    }
}
