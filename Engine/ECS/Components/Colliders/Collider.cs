namespace WoopWoop
{
    [RequireComponent(typeof(Transform))]
    public class Collider : Component
    {
        public virtual bool IsCollidingWith(Collider other) { return false; }
        public bool IsCollidingWith(Entity other)
        {
            if (other == null) return false;
            return IsCollidingWith(other.GetComponent<BoxCollider>());
        }
        private Entity[] GetCollidingEntitiesFromList(List<Entity> allEntities)
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
            return GetCollidingEntitiesFromList(Entity.GetEntitiesWithTag(tag).ToList());
        }
        public Entity[] GetAllCollidingEntities()
        {
            return GetCollidingEntitiesFromList(Entity.GetAllEntities().ToList());
        }
    }
}