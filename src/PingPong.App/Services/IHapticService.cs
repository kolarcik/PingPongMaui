namespace PingPong.App.Services;

public interface IHapticService
{
    void Vibrate(HapticFeedbackType type = HapticFeedbackType.Click);
}

public enum HapticFeedbackType
{
    Click,
    LongPress,
    Success,
    Warning
}
