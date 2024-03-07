using System.Collections.Generic;
namespace WoopWoop
{
    public class Entity
    {
        private List<Component> components;
        public Transform transform;
        public Entity()
        {
            components = new List<Component>();
            transform = AddComponent<Transform>();
        }
        public virtual void Update() { }
        public virtual void Draw() { }

        public T AddComponent<T>() where T : Component, new()
        {
            T comp = new T();
            comp.Attach(this);
            components.Add(comp);
            return GetComponent<T>();
        }
        public T GetComponent<T>() where T : Component
        {
            return components.Find(c => c.GetType() == typeof(T)) as T;
        }
        public void InternalUpdate()
        {
            foreach (Component c in GetComponents())
            {
                c.Update();
            }
        }
        public Component[] GetComponents()
        {
            return components.ToArray();
        }
    }
}