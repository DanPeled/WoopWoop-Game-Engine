using System.Numerics;
using ZeroElectric.Vinculum;

namespace WoopWoop
{
    [RequireComponent(typeof(Transform))]
    public class Camera : Component
    {
        public Camera2D camera;
        private static Camera mainCamera;
        public int width, height;
        private bool isMain = false;
        public Color backgroundColor = Raylib.WHITE;
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
            camera = new Camera2D();
            camera.offset = new Vector2(transform.Position.X + centerX, transform.Position.Y + centerY);
            camera.target = new Vector2(centerX, centerY);
            camera.rotation = transform.Angle;
            camera.zoom = 1f;


            if (mainCamera == default)
            {
                mainCamera = this;
            }

            transform.onTransformChanged += () =>
                    {
                        camera.rotation = transform.Angle;
                    };
        }
        public override void Update(float deltaTime)
        {
            Raylib.BeginMode2D(camera);
            transform.Position = camera.target;
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