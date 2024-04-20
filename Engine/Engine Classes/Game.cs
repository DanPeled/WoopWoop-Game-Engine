namespace WoopWoop
{
    /// <summary>
    /// Represents the base class for a game.
    /// </summary>
    public abstract class Game
    {
        /// <summary>
        /// Called when the game starts.
        /// </summary>
        public virtual void Start() { }

        /// <summary>
        /// Called every frame to update the game logic.
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// Called when the game is stopped or terminated.
        /// </summary>
        public virtual void OnStop() { }
    }
}
