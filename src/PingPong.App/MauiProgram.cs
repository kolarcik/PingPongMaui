using Microsoft.Extensions.Logging;
using PingPong.App.Services;

namespace PingPong.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<IAudioService, AudioService>();
        builder.Services.AddSingleton<IHapticService, HapticService>();
        builder.Services.AddSingleton<IPreferencesService, PreferencesService>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
