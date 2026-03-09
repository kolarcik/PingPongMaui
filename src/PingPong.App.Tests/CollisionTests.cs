using PingPong.App.Game.Core;
using PingPong.App.Game.Systems;

namespace PingPong.App.Tests;

public class CollisionTests
{
    private static GameState MakeRunningState(float arenaWidth = 800f, float arenaHeight = 600f)
    {
        var state = new GameState();
        state.Initialize(arenaWidth, arenaHeight);
        return state;
    }

    private static CollisionSystem MakeSystem() =>
        new CollisionSystem(new BallMovementSystem());

    // ── Paddle collisions ────────────────────────────────────────────────────

    [Fact]
    public void Update_PlayerPaddleCollision_BallBouncesUpward()
    {
        var system = MakeSystem();
        var state = MakeRunningState();

        // Place ball on top of player paddle, moving down
        var paddle = state.PlayerPaddle; // Y = 600 - 30 = 570
        state.Ball.Reset(paddle.X, paddle.Y - paddle.Height / 2 - state.Ball.Radius + 1f);
        state.Ball.VelocityX = 0f;
        state.Ball.VelocityY = 200f; // moving down (positive Y = towards player paddle)

        system.Update(state);

        Assert.True(state.Ball.VelocityY < 0, "Ball should bounce upward (negative VelocityY) after hitting player paddle");
    }

    [Fact]
    public void Update_OpponentPaddleCollision_BallBouncesDownward()
    {
        var system = MakeSystem();
        var state = MakeRunningState();

        // Place ball on bottom of opponent paddle, moving up
        var paddle = state.OpponentPaddle; // Y = 30
        state.Ball.Reset(paddle.X, paddle.Y + paddle.Height / 2 + state.Ball.Radius - 1f);
        state.Ball.VelocityX = 0f;
        state.Ball.VelocityY = -200f; // moving up (towards opponent paddle)

        system.Update(state);

        Assert.True(state.Ball.VelocityY > 0, "Ball should bounce downward (positive VelocityY) after hitting opponent paddle");
    }

    // ── Scoring ──────────────────────────────────────────────────────────────

    [Fact]
    public void Update_BallPassesTopOfArena_PlayerScores()
    {
        var system = MakeSystem();
        var state = MakeRunningState();
        state.Ball.Reset(400f, 5f); // near top; Radius=10, so Y-Radius = -5 <= 0
        state.Ball.VelocityX = 0f;
        state.Ball.VelocityY = -200f;

        system.Update(state);

        Assert.Equal(1, state.Score.PlayerScore);
        Assert.Equal(0, state.Score.OpponentScore);
    }

    [Fact]
    public void Update_BallPassesBottomOfArena_OpponentScores()
    {
        var system = MakeSystem();
        var state = MakeRunningState(800f, 600f);
        state.Ball.Reset(400f, 595f); // near bottom; Y+Radius = 605 >= 600
        state.Ball.VelocityX = 0f;
        state.Ball.VelocityY = 200f;

        system.Update(state);

        Assert.Equal(0, state.Score.PlayerScore);
        Assert.Equal(1, state.Score.OpponentScore);
    }

    [Fact]
    public void Update_ScoreIncrement_ReturnsTrueWhenScored()
    {
        var system = MakeSystem();
        var state = MakeRunningState();
        state.Ball.Reset(400f, 5f); // near top
        state.Ball.VelocityY = -200f;

        bool result = system.Update(state);

        Assert.True(result);
    }

    [Fact]
    public void Update_NoScore_ReturnsFalse()
    {
        var system = MakeSystem();
        var state = MakeRunningState();
        state.Ball.Reset(400f, 300f); // center, safe
        state.Ball.VelocityX = 100f;
        state.Ball.VelocityY = 100f;

        bool result = system.Update(state);

        Assert.False(result);
    }

    // ── Game over ─────────────────────────────────────────────────────────────

    [Fact]
    public void Update_PlayerReachesWinningScore_StopsGame()
    {
        var system = MakeSystem();
        var state = MakeRunningState();
        state.Score.PlayerScore = state.Score.WinningScore - 1;

        // Ball at top → player scores last point
        state.Ball.Reset(400f, 5f);
        state.Ball.VelocityY = -200f;

        system.Update(state);

        Assert.False(state.IsRunning);
        Assert.Equal(state.Score.WinningScore, state.Score.PlayerScore);
    }

    [Fact]
    public void Update_AfterScoring_BallResetToCenter_WhenNotGameOver()
    {
        var system = MakeSystem();
        var state = MakeRunningState(800f, 600f);
        // Ensure not near game over
        state.Score.PlayerScore = 0;

        state.Ball.Reset(400f, 5f);
        state.Ball.VelocityY = -200f;

        system.Update(state);

        // Ball should be reset to arena center
        Assert.Equal(400f, state.Ball.X);
        Assert.Equal(300f, state.Ball.Y);
    }

    // ── Paused ────────────────────────────────────────────────────────────────

    [Fact]
    public void Update_WhenPaused_DoesNothing()
    {
        var system = MakeSystem();
        var state = MakeRunningState();
        state.Ball.Reset(400f, 5f);
        state.Ball.VelocityY = -200f;
        state.IsPaused = true;

        bool result = system.Update(state);

        Assert.False(result);
        Assert.Equal(0, state.Score.PlayerScore);
    }

    // ── Angle adjustment ─────────────────────────────────────────────────────

    [Fact]
    public void Update_CenterPaddleHit_BallTrajectorymostlyVertical()
    {
        var system = MakeSystem();
        var state = MakeRunningState();

        var paddle = state.PlayerPaddle;
        // Place ball exactly at center of player paddle
        state.Ball.Reset(paddle.X, paddle.Y - paddle.Height / 2 - state.Ball.Radius + 1f);
        state.Ball.VelocityX = 0f;
        state.Ball.VelocityY = 200f;

        system.Update(state);

        // After a center hit, |VelocityX| should be small relative to speed
        float speed = MathF.Sqrt(state.Ball.VelocityX * state.Ball.VelocityX +
                                  state.Ball.VelocityY * state.Ball.VelocityY);
        float vxFraction = Math.Abs(state.Ball.VelocityX) / speed;
        Assert.True(vxFraction < 0.2f, $"Center hit should give mostly vertical trajectory, VX fraction={vxFraction}");
    }

    [Fact]
    public void Update_EdgePaddleHit_BallTrajectorySteepAngle()
    {
        var system = MakeSystem();
        var state = MakeRunningState();

        var paddle = state.PlayerPaddle;
        // Place ball at the far right edge of the paddle
        float edgeX = paddle.X + paddle.Width / 2 - 1f;
        state.Ball.Reset(edgeX, paddle.Y - paddle.Height / 2 - state.Ball.Radius + 1f);
        state.Ball.VelocityX = 0f;
        state.Ball.VelocityY = 200f;

        system.Update(state);

        float speed = MathF.Sqrt(state.Ball.VelocityX * state.Ball.VelocityX +
                                  state.Ball.VelocityY * state.Ball.VelocityY);
        float vxFraction = Math.Abs(state.Ball.VelocityX) / speed;
        Assert.True(vxFraction > 0.5f, $"Edge hit should give angled trajectory, VX fraction={vxFraction}");
    }
}
