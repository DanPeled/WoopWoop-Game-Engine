using ZeroElectric.Vinculum;

namespace WoopWoop.Audio
{
    /// <summary>
    /// Represents an audio clip that can be played.
    /// </summary>
    public class AudioClip
    {
        /// <summary>
        /// The sound clip to be played.
        /// </summary>
        public Sound clip;

        /// <summary>
        /// The volume of the audio clip.
        /// </summary>
        public byte volume;

        /// <summary>
        /// Initializes a new instance of the AudioClip class with the specified sound clip and volume.
        /// </summary>
        /// <param name="clip">The sound clip to be played.</param>
        /// <param name="volume">The volume of the audio clip (default is 100).</param>
        public AudioClip(Sound clip, byte volume = 100)
        {
            this.clip = clip;
            this.volume = volume;
        }

        /// <summary>
        /// Initializes a new instance of the AudioClip class with the sound clip loaded from the specified file path and volume.
        /// </summary>
        /// <param name="clipPath">The path to the sound clip file.</param>
        /// <param name="volume">The volume of the audio clip (default is 100).</param>
        public AudioClip(string clipPath, byte volume = 100) : this(Raylib.LoadSound(clipPath), volume) { }

        /// <summary>
        /// Plays the audio clip once.
        /// </summary>
        public void PlayOnce()
        {
            Raylib.SetSoundVolume(clip, (float)volume / 100); // Convert volume to a range between 0.0 and 1.0
            Raylib.PlaySound(clip);
        }

        /// <summary>
        /// Finalizes an instance of the AudioClip class.
        /// </summary>
        ~AudioClip()
        {
            Raylib.UnloadSound(clip);
        }
    }
}
