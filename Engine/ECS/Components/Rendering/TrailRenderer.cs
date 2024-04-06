using System.Diagnostics;
using System.Numerics;
using ZeroElectric.Vinculum;
using WoopWoop.UI;

namespace WoopWoop
{
    public class TrailRenderer : Component
    {
        private List<Vector2> segments;
        public float lastsFor = 10;
        private Stopwatch stopwatch;
        public float lineThickness = 5f;
        public override void Awake()
        {
            segments = new();
            stopwatch = new();
            stopwatch.Start();
        }
        public override void Update(float deltaTime)
        {
            // TODO: Figure out why it not clearing out proprely
            segments.Add(transform.Position);
            if (stopwatch.ElapsedMilliseconds >= lastsFor) // Multiply by 1000 to convert seconds to milliseconds
            {
                segments.RemoveAt(0);
                stopwatch.Restart();
            }
            for (int i = 0; i < segments.Count; i++)
            {
                Vector2 segment = segments.ElementAt(i);
                Vector2 endSegment = i < segments.Count - 1 ? segments.ElementAt(i + 1) : segment;
                Raylib.DrawLineEx(segment, endSegment, lineThickness, Raylib.BLACK);
            }
        }
    }
}