using WoopWoopEngine;

public class ImageSpammer : Component
{
    public override void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            Entity e = new();
            e.AddComponent<TextureRenderer>().LoadFromPath("resources/r.png");
            Random r = new();
            int x = r.Next(30, 400);
            int y = r.Next(0, 400);
            e.transform.Position = new(x, y);
            Entity.Instantiate(e);
        }
    }
}