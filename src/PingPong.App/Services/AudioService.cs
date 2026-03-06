namespace PingPong.App.Services;

/// <summary>
/// Platform-neutral stub; platform-specific implementations live in Platforms/.
/// </summary>
public partial class AudioService : IAudioService
{
    public float Volume { get; set; } = 1.0f;

    public partial Task PlayAsync(string soundName);
    public partial Task StopAllAsync();
}
