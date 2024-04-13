namespace WoopWoop
{
    /// <summary>
    /// Represents a component that can be attached to an entity in the game world.
    /// </summary>
    public abstract class Component
    {
        /// <summary>
        /// The entity to which this component is attached.
        /// </summary>
        public Entity entity { get; private set; }

        /// <summary>
        /// The transform of the entity to which this component is attached.
        /// </summary>
        public Transform transform
        {
            get
            {
                return entity.transform;
            }
        }

        /// <summary>
        /// Determines whether the component is enabled.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Attaches the component to an entity.
        /// </summary>
        /// <param name="entity">The entity to attach the component to.</param>
        public void Attach(Entity entity)
        {
            this.entity = entity;
        }

        /// <summary>
        /// Awake is called when the component is initialized, regardless of whether or not the script is enabled.
        /// </summary>
        public virtual void Awake() { }

        /// <summary>
        /// Called when the component is started if it's enabled.
        /// </summary>
        public virtual void Start() { }

        /// <summary>
        /// Called every frame to update the component's state.
        /// </summary>
        public virtual void Update(float deltaTime) { }

        /// <summary>
        /// Called on the end of every frame.
        /// </summary>
        public virtual void OnEndOfFrame() { }

        /// <summary>
        /// Called when the component is removed.
        /// </summary>
        public virtual void Stop() { }

        /// <summary>
        /// Tells the renderer how to draw the object's editor gizmos.
        /// </summary>
        public virtual void OnDrawGizmo() { }
    }
}
