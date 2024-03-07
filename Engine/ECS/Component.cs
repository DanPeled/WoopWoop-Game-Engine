namespace WoopWoop
{
    public abstract class Component
    {
        public Entity entity { get; private set; }
        public void Attach(Entity entity)
        {
            this.entity = entity;
        }
        public virtual void Start() { }
        public virtual void Update() { }
    }
}