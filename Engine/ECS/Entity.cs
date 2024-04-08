using System;
using System.Collections.Generic;
using System.Drawing;
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
        public static Action<Entity> OnEntityDestroyed;
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                // Enable / disable all children
                transform.GetChildren()?.ToList()?.ForEach(t =>
                {
                    if (t is not null && t.entity.Enabled)
                        t.entity.Enabled = value;
                });

                enabled = value;
            }
        }
        private static List<Entity> entities = new();
        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        public Entity() : this(Vector2.Zero) { }
        public Entity(float x, float y) : this(new(x, y)) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="startPos">The initial position of the entity.</param>
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
        /// Adds a new component of type T to the entity.
        /// </summary>
        /// <typeparam name="T">The type of component to add.</typeparam>
        /// <returns>The added component.</returns>
        public T AddComponent<T>() where T : Component, new()
        {
            T comp = new();
            return AddComponent(comp);
        }

        /// <summary>
        /// Adds a specific component instance to the entity.
        /// </summary>
        /// <typeparam name="T">The type of the component to add.</typeparam>
        /// <param name="comp">The instance of the component to add.</param>
        /// <returns>The added component.</returns>
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
                    if (requiredComponents.All(HasComponentOfType))
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

        /// <summary>
        /// Checks if the entity has a component of the specified type attached.
        /// </summary>
        /// <typeparam name="T">The type of component to check for.</typeparam>
        /// <returns>True if the entity has a component of the specified type, otherwise false.</returns>
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
        /// <summary>
        /// Instantiates an entity by adding it to the list of entities and initializing its components.
        /// </summary>
        /// <param name="entity">The entity to instantiate.</param>
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

        /// <summary>
        /// Retrieves an array containing all entities in the game.
        /// </summary>
        /// <returns>An array containing all entities.</returns>
        public static Entity[] GetAllEntities()
        {
            return entities.ToArray() is null ? new Entity[0] : entities.ToArray();
        }

        /// <summary>
        /// Retrieves an entity with the specified UUID.
        /// </summary>
        /// <param name="uuid">The UUID of the entity to retrieve.</param>
        /// <returns>The entity with the specified UUID, or null if not found.</returns>
        public static Entity GetEntityWithUUID(string uuid)
        {
            return entities.Find(e => e.ID.Equals(uuid));
        }

        /// <summary>
        /// Retrieves all entities with the specified tag.
        /// </summary>
        /// <param name="tag">The tag to search for.</param>
        /// <returns>An array containing all entities with the specified tag.</returns>
        public static Entity[] GetEntitiesWithTag(string tag)
        {
            return entities.FindAll(e => e.tag.Equals(tag)).ToArray();
        }

        /// <summary>
        /// Destroys an entity, removing it from the list of entities and stopping its components.
        /// </summary>
        /// <param name="entity">The entity to destroy.</param>
        public static void Destroy(Entity entity)
        {
            OnEntityDestroyed?.Invoke(entity);
            var entitiesCopy = new List<Entity>(entities);
            if (entity.transform != null)
            {
                foreach (Transform child in entity.transform.GetChildren())
                {
                    if (child?.entity != null)
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

        /// <summary>
        /// Stops all components attached to an entity.
        /// </summary>
        /// <param name="entity">The entity whose components to stop.</param>
        private static void StopComponents(Entity entity)
        {
            foreach (Component c in entity.GetComponents())
            {
                entity.RemoveComponent(c.GetType());
            }
        }

        /// <summary>
        /// Creates a new entity creator.
        /// </summary>
        /// <returns>A new instance of the <see cref="EntityCreator"/> class.</returns>
        public static EntityCreator CreateEntity()
        {
            return new EntityCreator();
        }


        /// <summary>
        /// Class responsible for creating entities and configuring their properties.
        /// </summary>
        public class EntityCreator
        {
            private Entity entityToCreate;

            /// <summary>
            /// Initializes a new instance of the <see cref="EntityCreator"/> class.
            /// </summary>
            public EntityCreator()
            {
                entityToCreate = new Entity();
            }

            /// <summary>
            /// Creates and returns the entity configured by this creator.
            /// </summary>
            /// <returns>The created entity.</returns>
            public Entity Create()
            {
                return entityToCreate;
            }

            /// <summary>
            /// Adds a component of type T to the entity.
            /// </summary>
            /// <typeparam name="T">The type of component to add.</typeparam>
            /// <returns>This <see cref="EntityCreator"/> instance.</returns>
            public EntityCreator AddComponent<T>() where T : Component, new()
            {
                entityToCreate.AddComponent<T>();
                return this;
            }

            /// <summary>
            /// Sets the position of the entity.
            /// </summary>
            /// <param name="position">The position to set.</param>
            /// <returns>This <see cref="EntityCreator"/> instance.</returns>
            public EntityCreator SetPosition(Vector2 position)
            {
                entityToCreate.transform.Position = position;
                return this;
            }

            /// <summary>
            /// Sets the scale of the entity.
            /// </summary>
            /// <param name="scale">The scale to set.</param>
            /// <returns>This <see cref="EntityCreator"/> instance.</returns>
            public EntityCreator SetScale(Vector2 scale)
            {
                entityToCreate.transform.Scale = scale;
                return this;
            }

            /// <summary>
            /// Sets the position and scale of the entity based on a rectangle.
            /// </summary>
            /// <param name="rectangle">The rectangle defining position and scale.</param>
            /// <returns>This <see cref="EntityCreator"/> instance.</returns>
            public EntityCreator SetTransformByRectangle(Rectangle rectangle)
            {
                entityToCreate.transform.Position = new Vector2(rectangle.X, rectangle.Y);
                entityToCreate.transform.Scale = new Vector2(rectangle.Width, rectangle.Height);
                return this;
            }

            /// <summary>
            /// Sets the angle of the entity.
            /// </summary>
            /// <param name="angle">The angle to set.</param>
            /// <returns>This <see cref="EntityCreator"/> instance.</returns>
            public EntityCreator SetAngle(float angle)
            {
                entityToCreate.transform.Angle = angle;
                return this;
            }
            /// <summary>
            /// Sets the tag for the entity.
            /// <param name="tag">The requested tag</param>
            /// <returns>This <see cref="EntityCreator"/> instance. </returns>
            public EntityCreator Tag(string tag)
            {
                entityToCreate.tag = tag;
                return this;
            }
        }
    }
}
