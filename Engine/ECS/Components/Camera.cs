using System.Numerics;
using WoopWoopEditor;
using ZeroElectric.Vinculum;

namespace WoopWoopEngine
{
    /// <summary>
    /// Represents a camera that can be used to view the scene.
    /// </summary>
    [RequireComponent(typeof(Transform))]
    public class Camera : Component
    {
        /// <summary>
        /// The 2D camera.
        /// </summary>
        public Camera2D camera;

        private static Camera mainCamera;

        /// <summary>
        /// The width of the camera.
        /// </summary>
        public int width, height;

        private bool isMain = false;

        /// <summary>
        /// The background color of the camera view.
        /// </summary>
        public Color backgroundColor = Raylib.WHITE;

        /// <summary>
        /// The center X position of the camera.
        /// </summary>
        public readonly float centerX = WoopWoop.screenWidth / 2;

        /// <summary>
        /// The center Y position of the camera.
        /// </summary>
        public readonly float centerY = WoopWoop.screenHeight / 2;

        /// <summary>
        /// Indicates whether this camera is the main camera.
        /// </summary>
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

        /// <summary>
        /// Called when the component starts.
        /// </summary>
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

        /// <summary>
        /// Called every frame.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last frame.</param>
        public override void Update(float deltaTime)
        {
            Raylib.BeginMode2D(camera);
            // transform.Position = camera.target;
        }

        /// <summary>
        /// Called at the end of each frame.
        /// </summary>
        public override void OnEndOfFrame()
        {
            Raylib.EndMode2D();
#if DEBUG
            if (WoopWoop.IsInDebugMenu)
            {
                camera.offset = new Vector2(transform.Position.X + centerX + Editor.dividingLineX, transform.Position.Y + centerY);
            }
            else
            {
                camera.offset = new Vector2(transform.Position.X + centerX, transform.Position.Y + centerY);
            }
#endif
            // TODO: Fix pointer position when changing camera position & rotation
        }

        /// <summary>
        /// Gets the main camera.
        /// </summary>
        /// <returns>The main camera.</returns>
        public static Camera Main()
        {
            return mainCamera;
        }
    }
}
