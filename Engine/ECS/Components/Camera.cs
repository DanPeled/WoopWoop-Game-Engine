using System.Numerics;
using Raylib_cs;

namespace WoopWoop
{
    [RequireComponent(typeof(Transform))]
    public class Camera : Component
    {
        public Camera2D camera;
        private static Camera mainCamera;
        public int width, height;
        private bool isMain = false;
        public bool IsMain
        {
            get
            {
                return isMain;
            }
            set
            {
                isMain = value;
                mainCamera = this;
            }
        }
        public override void Start()
        {
            // Calculate the center of the screen
            float centerX = WoopWoopEngine.screenWidth / 2;
            float centerY = WoopWoopEngine.screenHeight / 2;

            // Set camera position to center of the screen
            camera = new Camera2D(
                new Vector2(transform.Position.X + centerX, transform.Position.Y + centerY),
                new Vector2(centerX, centerY),
                transform.Angle,
                1.0f
            );

            if (mainCamera == default)
            {
                mainCamera = this;
            }

            transform.onTransformChanged += () =>
            {
                camera.Rotation = transform.Angle;
            };
        }
        public override void Update(float deltaTime)
        {
            Raylib.BeginMode2D(camera);
            transform.Position = camera.Target;
        }
        public override void OnEndOfFrame()
        {
            Raylib.EndMode2D();
        }
        public static Camera Main()
        {
            return mainCamera;
        }
    }
}