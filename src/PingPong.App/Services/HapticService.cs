namespace PingPong.App.Services;

/// <summary>
/// Platform-neutral stub; platform-specific implementations live in Platforms/.
/// </summary>
public partial class HapticService : IHapticService
{
    public partial void Vibrate(HapticFeedbackType type);
}
