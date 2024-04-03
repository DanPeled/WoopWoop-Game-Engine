using Raylib_cs;
using WoopWoop;

public class LineRendererDemo : Game
{
    Entity line;
    LineRenderer lineRenderer;
    public override void Start()
    {
        line = new(new(WoopWoopEngine.screenWidth / 2, WoopWoopEngine.screenHeight / 2));
        lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.isEndPositionRelative = false;
        WoopWoopEngine.Instantiate(line);
    }
    public override void Update()
    {
        lineRenderer.endPosition = Raylib.GetMousePosition();
        lineRenderer.thickness += Raylib.GetMouseWheelMove() / 5;
    }
}