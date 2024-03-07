using System.Numerics;
using WoopWoop;
namespace WoopWoop;

public class DemoGame : Game
{

    public override void Start()
    {
        Entity e = new();
        e.transform.position = new Vector2(100, 40);
        e.AddComponent<Renderer>();
        WoopWoopEngine.Instantiate(e);
    }
    public override void Update()
    {

    }
}