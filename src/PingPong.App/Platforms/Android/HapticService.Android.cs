using Android.OS;
using Android.Content;

namespace PingPong.App.Services;

public partial class HapticService
{
    public partial void Vibrate(HapticFeedbackType type)
    {
        var vibrator = Platform.AppContext.GetSystemService(Context.VibratorService) as Vibrator;
        if (vibrator is null || !vibrator.HasVibrator) return;

        long durationMs = type switch
        {
            HapticFeedbackType.LongPress => 50,
            HapticFeedbackType.Success   => 30,
            HapticFeedbackType.Warning   => 40,
            _                            => 10,
        };

        if (OperatingSystem.IsAndroidVersionAtLeast(26))
            vibrator.Vibrate(VibrationEffect.CreateOneShot(durationMs, VibrationEffect.DefaultAmplitude));
        else
#pragma warning disable CA1422
            vibrator.Vibrate(durationMs);
#pragma warning restore CA1422
    }
}
