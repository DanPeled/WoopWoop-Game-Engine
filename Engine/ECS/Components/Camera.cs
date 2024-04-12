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
        public readonly float centerX = WoopWoopEngine.screenWidth / 2;
        public readonly float centerY = WoopWoopEngine.screenHeight / 2;
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
            // Set camera position to center of the screen
            camera = new Camera2D
            {
                offset = new Vector2(transform.Position.X + centerX, transform.Position.Y + centerY),
                target = new Vector2(centerX, centerY),
                rotation = transform.Angle,
                zoom = 1f
            };


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
            // transform.Position = camera.target;

        }
        public override void OnEndOfFrame()
        {
            Raylib.EndMode2D();
#if DEBUG
            if (WoopWoopEngine.IsInDebugMenu)
            {
                camera.offset = new Vector2(transform.Position.X + centerX + Editor.Editor.dividingLineX, transform.Position.Y + centerY);
            }
            else
            {
                camera.offset = new Vector2(transform.Position.X + centerX, transform.Position.Y + centerY);
            }
#endif
            // TODO: Fix pointer collision with objects when changing camera position
        }
        public static Camera Main()
        {
            return mainCamera;
        }
    }
}