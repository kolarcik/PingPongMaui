# Architecture

## Project Structure

```
src/
├── PingPong.sln
├── PingPong.App/
│   ├── PingPong.App.csproj
│   ├── MauiProgram.cs
│   ├── App.xaml(.cs)
│   ├── AppShell.xaml(.cs)
│   ├── Game/
│   │   ├── Core/          ← Ball, Paddle, Arena, ScoreState, GameState
│   │   └── Systems/       ← GameLoop, CollisionSystem, AISystem, InputSystem
│   ├── Views/             ← GamePage, GameDrawable
│   ├── ViewModels/        ← GameViewModel
│   ├── Services/          ← IAudioService, AudioService
│   ├── Platforms/         ← Android & iOS specifics
│   └── Resources/         ← Assets, sounds, images
└── PingPong.App.Tests/    ← Unit tests
```

## Principles

- **MVVM**: Views bind to ViewModels; ViewModels expose GameState
- **Separation**: Game/Core and Game/Systems are pure C# — no MAUI dependency
- **DI**: Services registered in MauiProgram.cs and injected
- **Rendering**: MAUI GraphicsView with IDrawable for 2D canvas
- **Game Loop**: IDispatcherTimer at ~60 FPS driving GameLoop.Tick()