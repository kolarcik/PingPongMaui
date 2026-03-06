using Android.Media;

namespace PingPong.App.Services;

public partial class AudioService
{
    private SoundPool? _soundPool;
    private readonly Dictionary<string, int> _soundIds = new();

    public partial async Task PlayAsync(string soundName)
    {
        // TODO: load sound from raw resources and play via SoundPool
        await Task.CompletedTask;
    }

    public partial async Task StopAllAsync()
    {
        _soundPool?.AutoPause();
        await Task.CompletedTask;
    }
}
