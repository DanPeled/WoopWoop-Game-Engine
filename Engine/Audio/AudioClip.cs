using ZeroElectric.Vinculum;

namespace WoopWoop.Audio
{
    public struct AudioClip
    {
        Sound clip;
        byte volume;

        public AudioClip(Sound clip, byte volume = 100)
        {
            this.clip = clip;
            this.volume = volume;
        }

        public void PlayOnce()
        {
            Raylib.SetSoundVolume(clip, (float)volume / 100); // Convert volume to a range between 0.0 and 1.0
            Raylib.PlaySound(clip);
        }
    }
}
