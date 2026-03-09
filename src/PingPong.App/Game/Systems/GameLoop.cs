namespace PingPong.App.Game.Systems;

using PingPong.App.Game.Core;

public class GameLoop
{
    public GameState State { get; } = new();
    public InputSystem Input { get; } = new();
    public AISystem AI { get; } = new();
    public BallMovementSystem BallMovement { get; } = new();
    public CollisionSystem Collision { get; }

    public event Action? OnScored;
    public event Action? OnGameOver;

    public GameLoop()
    {
        Collision = new CollisionSystem(BallMovement);
    }

    public void Start(float arenaWidth, float arenaHeight)
    {
        State.Initialize(arenaWidth, arenaHeight);
        BallMovement.LaunchBall(State);
    }

    public void Tick(float deltaTime)
    {
        if (!State.IsRunning || State.IsPaused) return;

        Input.Update(State, deltaTime);
        AI.Update(State, deltaTime);
        BallMovement.Update(State, deltaTime);

        bool scored = Collision.Update(State);
        if (scored)
        {
            if (State.Score.IsGameOver)
                OnGameOver?.Invoke();
            else
                OnScored?.Invoke();
        }
    }

    public void Pause() => State.IsPaused = true;
    public void Resume() => State.IsPaused = false;
    public void TogglePause() => State.IsPaused = !State.IsPaused;

    public void Restart(float arenaWidth, float arenaHeight)
    {
        AI.Reset();
        Start(arenaWidth, arenaHeight);
    }
}
