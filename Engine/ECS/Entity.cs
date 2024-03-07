using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WoopWoop
{
    public class Entity
    {
        private List<Component> components;
        public Transform transform;
        private readonly object componentsLock = new object();

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
            lock (componentsLock)
            {
                components.Add(comp);
            }
            return GetComponent<T>();
        }

        public T GetComponent<T>() where T : Component
        {
            lock (componentsLock)
            {
                return components.Find(c => c.GetType() == typeof(T)) as T;
            }
        }

        public void InternalUpdate()
        {
            Parallel.ForEach(GetComponents(), c =>
            {
                c.Update();
            });
        }

        public Component[] GetComponents()
        {
            lock (componentsLock)
            {
                return components.ToArray();
            }
        }
    }
}
