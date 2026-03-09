using PingPong.App.Game.Core;
using PingPong.App.Game.Systems;

namespace PingPong.App.Tests;

public class BallMovementTests
{
    private static GameState MakeRunningState(float arenaWidth = 800f, float arenaHeight = 600f)
    {
        var state = new GameState();
        state.Initialize(arenaWidth, arenaHeight);
        return state;
    }

    // ── Movement ─────────────────────────────────────────────────────────────

    [Fact]
    public void Update_MovesBallByVelocityTimesDeltaTime()
    {
        var system = new BallMovementSystem();
        var state = MakeRunningState();
        state.Ball.Reset(400f, 300f);
        state.Ball.VelocityX = 100f;
        state.Ball.VelocityY = 50f;

        system.Update(state, 0.1f);

        Assert.Equal(410f, state.Ball.X, precision: 4);
        Assert.Equal(305f, state.Ball.Y, precision: 4);
    }

    [Fact]
    public void Update_DoesNotMoveBall_WhenPaused()
    {
        var system = new BallMovementSystem();
        var state = MakeRunningState();
        state.Ball.Reset(400f, 300f);
        state.Ball.VelocityX = 200f;
        state.Ball.VelocityY = 200f;
        state.IsPaused = true;

        system.Update(state, 0.1f);

        Assert.Equal(400f, state.Ball.X);
        Assert.Equal(300f, state.Ball.Y);
    }

    [Fact]
    public void Update_DoesNotMoveBall_WhenNotRunning()
    {
        var system = new BallMovementSystem();
        var state = MakeRunningState();
        state.Ball.Reset(400f, 300f);
        state.Ball.VelocityX = 200f;
        state.Ball.VelocityY = 200f;
        state.IsRunning = false;

        system.Update(state, 0.1f);

        Assert.Equal(400f, state.Ball.X);
        Assert.Equal(300f, state.Ball.Y);
    }

    // ── Wall collisions ───────────────────────────────────────────────────────

    [Fact]
    public void Update_BouncesOffLeftWall_VelocityXBecomesPositive()
    {
        var system = new BallMovementSystem();
        var state = MakeRunningState();
        // Place ball just past the left wall
        state.Ball.Reset(2f, 300f);   // Radius=10, so 2 < 10 → collision
        state.Ball.VelocityX = -200f;
        state.Ball.VelocityY = 0f;

        system.Update(state, 0.016f);

        Assert.True(state.Ball.VelocityX > 0, "VelocityX should be positive after left-wall bounce");
        Assert.True(state.Ball.X >= state.Ball.Radius, "Ball X should be clamped to Radius");
    }

    [Fact]
    public void Update_BouncesOffRightWall_VelocityXBecomesNegative()
    {
        var system = new BallMovementSystem();
        var state = MakeRunningState(800f, 600f);
        // Place ball just past the right wall
        state.Ball.Reset(795f, 300f);  // 800 - 10 = 790 → 795 > 790 → collision
        state.Ball.VelocityX = 200f;
        state.Ball.VelocityY = 0f;

        system.Update(state, 0.016f);

        Assert.True(state.Ball.VelocityX < 0, "VelocityX should be negative after right-wall bounce");
        Assert.True(state.Ball.X <= state.Arena.Width - state.Ball.Radius,
            "Ball X should be clamped to Width - Radius");
    }

    // ── LaunchBall ────────────────────────────────────────────────────────────

    [Fact]
    public void LaunchBall_SetsNonZeroVelocity()
    {
        var system = new BallMovementSystem();
        var state = MakeRunningState();
        state.Ball.VelocityX = 0f;
        state.Ball.VelocityY = 0f;

        system.LaunchBall(state);

        var speed = MathF.Sqrt(state.Ball.VelocityX * state.Ball.VelocityX +
                               state.Ball.VelocityY * state.Ball.VelocityY);
        Assert.True(speed > 0f, "Ball speed should be non-zero after LaunchBall");
    }

    [Fact]
    public void LaunchBall_TowardsPlayer_VelocityYIsPositive()
    {
        // Run multiple times because X direction is random; Y direction is deterministic
        var system = new BallMovementSystem();
        for (int i = 0; i < 10; i++)
        {
            var state = MakeRunningState();
            system.LaunchBall(state, towardsPlayer: true);
            Assert.True(state.Ball.VelocityY > 0,
                "VelocityY should be positive (towards player at bottom) when towardsPlayer=true");
        }
    }

    [Fact]
    public void LaunchBall_AwayFromPlayer_VelocityYIsNegative()
    {
        var system = new BallMovementSystem();
        for (int i = 0; i < 10; i++)
        {
            var state = MakeRunningState();
            system.LaunchBall(state, towardsPlayer: false);
            Assert.True(state.Ball.VelocityY < 0,
                "VelocityY should be negative (away from player) when towardsPlayer=false");
        }
    }

    // ── IncreaseSpeed ─────────────────────────────────────────────────────────

    [Fact]
    public void IncreaseSpeed_IncreasesSpeed()
    {
        var system = new BallMovementSystem();
        var ball = new Ball { VelocityX = 200f, VelocityY = 200f };
        float before = MathF.Sqrt(ball.VelocityX * ball.VelocityX + ball.VelocityY * ball.VelocityY);

        system.IncreaseSpeed(ball);

        float after = MathF.Sqrt(ball.VelocityX * ball.VelocityX + ball.VelocityY * ball.VelocityY);
        Assert.True(after > before, "Speed should increase after IncreaseSpeed()");
    }

    [Fact]
    public void IncreaseSpeed_DoesNotExceedMaxSpeed()
    {
        var system = new BallMovementSystem();
        // Start at 595, close to 600 cap; call many times
        var ball = new Ball { VelocityX = 595f, VelocityY = 0f };

        for (int i = 0; i < 20; i++)
            system.IncreaseSpeed(ball);

        float speed = MathF.Sqrt(ball.VelocityX * ball.VelocityX + ball.VelocityY * ball.VelocityY);
        Assert.True(speed <= 600f + 0.01f, $"Speed {speed} should not exceed MaxSpeed of 600");
    }
}
