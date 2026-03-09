using Microsoft.Extensions.Logging;

namespace PingPong;

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

        // TODO: Register game services here
        // builder.Services.AddSingleton<IGameEngine, GameEngine>();
        // builder.Services.AddSingleton<IPhysicsSystem, PhysicsSystem>();
        // builder.Services.AddSingleton<IInputSystem, InputSystem>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
