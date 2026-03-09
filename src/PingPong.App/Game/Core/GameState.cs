namespace PingPong.App.Game.Core;

public class GameState
{
    public Ball Ball { get; } = new();
    public Paddle PlayerPaddle { get; } = new();
    public Paddle OpponentPaddle { get; } = new();
    public Arena Arena { get; } = new();
    public ScoreState Score { get; } = new();
    public bool IsPaused { get; set; }
    public bool IsRunning { get; set; }

    public void Initialize(float arenaWidth, float arenaHeight)
    {
        Arena.Width = arenaWidth;
        Arena.Height = arenaHeight;

        PlayerPaddle.X = arenaWidth / 2;
        PlayerPaddle.Y = arenaHeight - Arena.PaddleMargin;

        OpponentPaddle.X = arenaWidth / 2;
        OpponentPaddle.Y = Arena.PaddleMargin;

        ResetBall();
        Score.Reset();
        IsPaused = false;
        IsRunning = true;
    }

    public void ResetBall()
    {
        Ball.Reset(Arena.Width / 2, Arena.Height / 2);
    }
}
