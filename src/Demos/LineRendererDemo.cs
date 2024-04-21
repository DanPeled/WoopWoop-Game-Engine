using ZeroElectric.Vinculum;
using WoopWoopEngine;

public class LineRendererDemo : Game
{
    Entity line;
    LineRenderer lineRenderer;
    public override void Start()
    {
        line = new(new(WoopWoop.screenWidth / 2, WoopWoop.screenHeight / 2));
        lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.isEndPositionRelative = false;
        Entity.Instantiate(line);
    }
    public override void Update()
    {
        lineRenderer.endPosition = Raylib.GetMousePosition();
        lineRenderer.thickness += Raylib.GetMouseWheelMove() / 5;
    }
}