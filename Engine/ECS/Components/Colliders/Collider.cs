namespace WoopWoop
{
    [RequireComponent(typeof(Transform))]
    public class Collider : Component
    {
        public virtual bool IsCollidingWith(Collider other) { return false; }
        public bool IsCollidingWith(Entity other) { return IsCollidingWith(other.GetComponent<BoxCollider>()); }
        private Entity[] GetCollidingEntities(List<Entity> allEntities)
        {
            List<Entity> collidingEntities = new();
            allEntities.Remove(entity);
            foreach (Entity e in allEntities)
            {
                if (IsCollidingWith(e))
                {
                    collidingEntities.Add(e);
                }
            }
            return collidingEntities.ToArray();
        }

        public Entity[] GetCollidingEntitiesWithTag(string tag)
        {
            return GetCollidingEntities(Entity.GetEntitiesWithTag(tag).ToList());
        }
        public Entity[] GetAllCollidingEntities()
        {
            return GetCollidingEntities(Entity.GetAllEntities().ToList());
        }
    }
}