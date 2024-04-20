namespace WoopWoop.Engine
{
    /// <summary>
    /// Represents a subsystem of the engine.
    /// </summary>
    public abstract class Subsystem
    {
        /// <summary>
        /// Gets or sets a value indicating whether this subsystem is enabled.
        /// </summary>
        public virtual bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether this subsystem is for debugging purposes only.
        /// </summary>
        public virtual bool DebugModeOnly { get; set; } = false;

        /// <summary>
        /// Initializes the subsystem.
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// Updates the subsystem.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Called when the subsystem is stopped.
        /// </summary>
        public abstract void OnStop();
    }
}
