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
        /// Called when the component is started.
        /// </summary>
        public virtual void Start() { }

        /// <summary>
        /// Called every frame to update the component's state.
        /// </summary>
        public virtual void Update(float deltaTime) { }

        /// <summary>
        /// Called when the component is removed
        /// </summary>

        public virtual void Stop()
        {

        }
        public virtual void OnDrawGizmo() { }
        // public virtual void OnCollisionEnter(Collider other) { }
        // public virtual void OnCollisionStay(Collider other) { }
        // public virtual void OnCollisionExit(Collider other) { }
    }
}
