using ZeroElectric.Vinculum;

namespace WoopWoopEngine
{
    public class AudioSubsystem : Subsystem
    {
        public override void Init()
        {
            Raylib.InitAudioDevice();
        }

        public override void OnStop()
        {
            Raylib.CloseAudioDevice();
        }

        public override void Update()
        {
            return;
        }
    }
}