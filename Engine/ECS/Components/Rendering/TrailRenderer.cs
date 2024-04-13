using System.Diagnostics;
using System.Numerics;
using ZeroElectric.Vinculum;

namespace WoopWoop
{
    /// <summary>
    /// Renders a trail behind an entity.
    /// </summary>
    public class TrailRenderer : Component
    {
        private List<Vector2> segments;
        public float lastsFor = 10;
        private Stopwatch stopwatch;
        public float lineThickness = 5f;

        /// <summary>
        /// Initializes the TrailRenderer.
        /// </summary>
        public override void Awake()
        {
            segments = new List<Vector2>();
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        /// <summary>
        /// Updates the TrailRenderer.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last update.</param>
        public override void Update(float deltaTime)
        {
            segments.Add(transform.Position);
            if (stopwatch.ElapsedMilliseconds >= lastsFor * 1000) // Convert seconds to milliseconds
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
