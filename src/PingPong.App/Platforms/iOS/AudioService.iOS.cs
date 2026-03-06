using AVFoundation;

namespace PingPong.App.Services;

public partial class AudioService
{
    private AVAudioPlayer? _player;

    public partial async Task PlayAsync(string soundName)
    {
        // TODO: load from Resources/Raw and play via AVAudioPlayer
        await Task.CompletedTask;
    }

    public partial async Task StopAllAsync()
    {
        _player?.Stop();
        await Task.CompletedTask;
    }
}
