using Raylib_cs;
namespace WoopWoop
{
    public class Renderer : Component
    {
        public override void Update()
        {
            Raylib.DrawText("Hello, world!",
             (int)entity.transform.position.X, (int)entity.transform.position.Y, 20, Color.Black);
        }
    }
}