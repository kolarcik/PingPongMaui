namespace PingPong.App.Services;

public interface IAudioService
{
    Task PlayAsync(string soundName);
    Task StopAllAsync();
    float Volume { get; set; }
}
