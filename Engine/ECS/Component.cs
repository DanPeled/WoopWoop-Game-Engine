using System.Reflection.Metadata.Ecma335;

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
        public Transform transform
        {
            get
            {
                return entity.transform;
            }
        }
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
        /// Awake is called when the component is initialised, regardless of whether or not the script is enabled.
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

        public virtual void OnEndOfFrame() { }

        /// <summary>
        /// Called when the component is removed
        /// </summary>

        public virtual void Stop()
        {

        }
        public virtual void OnDrawGizmo() { }
    }
}
