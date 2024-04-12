using System.Numerics;

namespace WoopWoop.UI
{
    public class VerticalGrid : Component
    {
        public float spacing = 10;
        public float spacingTop = 5;
        public override void Update(float deltaTime)
        {
            Transform[] children = transform.GetChildren();
            for (int i = 0; i < children.Length; i++)
            {
                float additionalSpacing = spacingTop;
                if (i > 0)
                {
                    additionalSpacing = children[i - 1].transform.Scale.Y;
                }
                children[i].transform.Position = new(children[i].transform.Position.X, transform.Position.Y + spacing * i + additionalSpacing);
                Entity.GetEntityWithUUID(children[i].entity.ID).transform = children[i].transform;
            }
        }
    }
}