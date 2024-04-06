using System.Numerics;
using ZeroElectric.Vinculum;

namespace WoopWoop
{
    public class LineRenderer : Renderer
    {
        public Vector2 endPosition;
        public bool isEndPositionRelative = true;
        public float thickness = 3;
        public override void Update(float deltaTime)
        {
            Raylib.DrawLineEx(transform.Position, isEndPositionRelative ? transform.Position - endPosition : endPosition, thickness, Color);
        }
    }
}