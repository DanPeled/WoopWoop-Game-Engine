using Raylib_cs;
using WoopWoop;

public class SATChecking : Game
{
    BoxCollider boxCollider1, boxCollider2;
    Entity box1;
    BasicShapeRenderer shapeRenderer1, shapeRenderer2;
    public override void Start()
    {
        box1 = new();
        boxCollider1 = box1.AddComponent<BoxCollider>();
        shapeRenderer1 = box1.AddComponent<BasicShapeRenderer>();
        Entity box2 = new();
        box1.transform.Scale = new(50, 50);
        box1.AddComponent<PlayerController>();
        // box1.AddComponent<TrailRenderer>();

        Entity.Instantiate(box1);
        boxCollider2 = box2.AddComponent<BoxCollider>();
        shapeRenderer2 = box2.AddComponent<BasicShapeRenderer>();
        box2.transform.Scale = new(10, 10);
        box2.transform.Position = new(200, 200);
        Entity.Instantiate(box2);

        Entity fpsText = new();
        fpsText.AddComponent<FpsText>();
        Entity.Instantiate(fpsText);
    }

    public override void Update()
    {
        // // box1.transform.Position = Raylib.GetMousePosition();
        if (boxCollider1.IsCollidingWith(boxCollider2))
        {
            shapeRenderer1.Color = Color.Gray;
        }
        else
        {
            shapeRenderer1.Color = Color.Black;
        }
    }
}