using System.Numerics;

namespace WoopWoop.UI
{
    public class VerticalGrid : Component
    {
        public float spacing = 10;
        public float spacingTop = 3;
        public override void Update(float deltaTime)
        {
            Transform[] children = transform.GetChildren();
            for (int i = 0; i < children.Length; i++)
            {
                children[i].transform.Position = new(children[i].transform.Position.X, transform.Position.Y + spacing * i + spacingTop);
                Entity.GetEntityWithUUID(children[i].entity.ID).transform = children[i].transform;
            }
        }
    }
}