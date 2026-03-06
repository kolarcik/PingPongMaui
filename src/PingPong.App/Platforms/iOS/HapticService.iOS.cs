using UIKit;

namespace PingPong.App.Services;

public partial class HapticService
{
    public partial void Vibrate(HapticFeedbackType type)
    {
        var style = type switch
        {
            HapticFeedbackType.Success   => UINotificationFeedbackType.Success,
            HapticFeedbackType.Warning   => UINotificationFeedbackType.Warning,
            HapticFeedbackType.LongPress => UINotificationFeedbackType.Success, // handled below
            _                            => UINotificationFeedbackType.Success,
        };

        if (type is HapticFeedbackType.Click or type is HapticFeedbackType.LongPress)
        {
            var impactStyle = type is HapticFeedbackType.LongPress
                ? UIImpactFeedbackStyle.Heavy
                : UIImpactFeedbackStyle.Light;
            var impact = new UIImpactFeedbackGenerator(impactStyle);
            impact.ImpactOccurred();
        }
        else
        {
            var notification = new UINotificationFeedbackGenerator();
            notification.NotificationOccurred(style);
        }
    }
}
