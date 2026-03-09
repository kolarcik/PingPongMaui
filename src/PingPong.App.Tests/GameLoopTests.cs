using PingPong.App.Game.Core;
using PingPong.App.Game.Systems;

namespace PingPong.App.Tests;

public class GameLoopTests
{
    // ── Start ─────────────────────────────────────────────────────────────────

    [Fact]
    public void Start_InitializesStateAndLaunchesBall()
    {
        var loop = new GameLoop();

        loop.Start(800f, 600f);

        Assert.True(loop.State.IsRunning);
        Assert.False(loop.State.IsPaused);
        Assert.Equal(800f, loop.State.Arena.Width);
        Assert.Equal(600f, loop.State.Arena.Height);

        float speed = MathF.Sqrt(loop.State.Ball.VelocityX * loop.State.Ball.VelocityX +
                                   loop.State.Ball.VelocityY * loop.State.Ball.VelocityY);
        Assert.True(speed > 0f, "Ball should have non-zero velocity after Start()");
    }

    // ── Tick ──────────────────────────────────────────────────────────────────

    [Fact]
    public void Tick_MovesBall_WhenRunning()
    {
        var loop = new GameLoop();
        loop.Start(800f, 600f);

        float startX = loop.State.Ball.X;
        float startY = loop.State.Ball.Y;

        loop.Tick(0.016f);

        bool moved = loop.State.Ball.X != startX || loop.State.Ball.Y != startY;
        Assert.True(moved, "Ball should move after Tick()");
    }

    [Fact]
    public void Tick_DoesNothing_WhenPaused()
    {
        var loop = new GameLoop();
        loop.Start(800f, 600f);
        loop.Pause();

        float startX = loop.State.Ball.X;
        float startY = loop.State.Ball.Y;

        loop.Tick(0.016f);

        Assert.Equal(startX, loop.State.Ball.X);
        Assert.Equal(startY, loop.State.Ball.Y);
    }

    [Fact]
    public void Tick_DoesNothing_WhenNotRunning()
    {
        var loop = new GameLoop();
        loop.Start(800f, 600f);
        loop.State.IsRunning = false;

        float startX = loop.State.Ball.X;
        float startY = loop.State.Ball.Y;

        loop.Tick(0.016f);

        Assert.Equal(startX, loop.State.Ball.X);
        Assert.Equal(startY, loop.State.Ball.Y);
    }

    // ── Pause / Resume / TogglePause ─────────────────────────────────────────

    [Fact]
    public void Pause_SetsPausedTrue()
    {
        var loop = new GameLoop();
        loop.Start(800f, 600f);

        loop.Pause();

        Assert.True(loop.State.IsPaused);
    }

    [Fact]
    public void Resume_SetsPausedFalse()
    {
        var loop = new GameLoop();
        loop.Start(800f, 600f);
        loop.Pause();

        loop.Resume();

        Assert.False(loop.State.IsPaused);
    }

    [Fact]
    public void TogglePause_TogglesState()
    {
        var loop = new GameLoop();
        loop.Start(800f, 600f);
        Assert.False(loop.State.IsPaused);

        loop.TogglePause();
        Assert.True(loop.State.IsPaused);

        loop.TogglePause();
        Assert.False(loop.State.IsPaused);
    }

    // ── Restart ───────────────────────────────────────────────────────────────

    [Fact]
    public void Restart_ResetsScoreAndLaunchesBall()
    {
        var loop = new GameLoop();
        loop.Start(800f, 600f);
        loop.State.Score.PlayerScore = 3;
        loop.State.Score.OpponentScore = 2;

        loop.Restart(800f, 600f);

        Assert.Equal(0, loop.State.Score.PlayerScore);
        Assert.Equal(0, loop.State.Score.OpponentScore);
        Assert.True(loop.State.IsRunning);

        float speed = MathF.Sqrt(loop.State.Ball.VelocityX * loop.State.Ball.VelocityX +
                                   loop.State.Ball.VelocityY * loop.State.Ball.VelocityY);
        Assert.True(speed > 0f, "Ball should have velocity after Restart()");
    }

    // ── Events ────────────────────────────────────────────────────────────────

    [Fact]
    public void Tick_FiresOnScored_WhenBallPassesEdge()
    {
        var loop = new GameLoop();
        loop.Start(800f, 600f);

        int scoredCount = 0;
        loop.OnScored += () => scoredCount++;

        // Manually position ball at top to trigger player score
        loop.State.Ball.Reset(400f, 5f);
        loop.State.Ball.VelocityX = 0f;
        loop.State.Ball.VelocityY = -300f;

        loop.Tick(0.016f);

        Assert.Equal(1, scoredCount);
    }

    [Fact]
    public void Tick_FiresOnGameOver_WhenWinningScoreReached()
    {
        var loop = new GameLoop();
        loop.Start(800f, 600f);
        loop.State.Score.PlayerScore = loop.State.Score.WinningScore - 1;

        int gameOverCount = 0;
        loop.OnGameOver += () => gameOverCount++;

        // Position ball to score the winning point
        loop.State.Ball.Reset(400f, 5f);
        loop.State.Ball.VelocityX = 0f;
        loop.State.Ball.VelocityY = -300f;

        loop.Tick(0.016f);

        Assert.Equal(1, gameOverCount);
        Assert.False(loop.State.IsRunning);
    }
}
