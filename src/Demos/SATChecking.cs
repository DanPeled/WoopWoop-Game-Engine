using Raylib_cs;
using WoopWoop;

public class SATChecking : Game
{
    BoxCollider boxCollider1, boxCollider2;
    Entity box1;
    public override void Start()
    {
        box1 = new();
        boxCollider1 = box1.AddComponent<BoxCollider>();
        box1.AddComponent<BasicShapeRenderer>();
        Entity box2 = new();
        box1.transform.Scale = new(50, 50);
        box1.AddComponent<PlayerController>();
        // box1.AddComponent<TrailRenderer>();

        WoopWoopEngine.Instantiate(box1);
        boxCollider2 = box2.AddComponent<BoxCollider>();
        box2.AddComponent<BasicShapeRenderer>();
        box2.transform.Scale = new(10, 10);
        box2.transform.Position = new(200, 200);
        WoopWoopEngine.Instantiate(box2);
    }

    public override void Update()
    {
        // // box1.transform.Position = Raylib.GetMousePosition();
        if (boxCollider1.IsCollidingWith(boxCollider2))
        {
            Console.WriteLine(boxCollider1.IsCollidingWith(boxCollider2));
        }
    }
}