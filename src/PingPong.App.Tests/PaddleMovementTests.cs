using PingPong.App.Game.Core;
using PingPong.App.Game.Systems;

namespace PingPong.App.Tests;

public class PaddleMovementTests
{
    [Fact]
    public void InputSystem_MovesPlayerPaddleTowardTargetX()
    {
        var state = new GameState();
        state.Initialize(800f, 600f);
        state.PlayerPaddle.X = 100f;

        var inputSystem = new InputSystem { TargetX = 300f };
        float deltaTime = 0.016f; // ~60 FPS

        inputSystem.Update(state, deltaTime);

        Assert.True(state.PlayerPaddle.X > 100f);
        Assert.True(state.PlayerPaddle.X <= 300f);
    }

    [Fact]
    public void InputSystem_ClampsPaddleWithinArenaBounds()
    {
        var state = new GameState();
        state.Initialize(800f, 600f);
        state.PlayerPaddle.X = 400f;
        float halfWidth = state.PlayerPaddle.Width / 2;

        var inputSystem = new InputSystem { TargetX = -100f };
        inputSystem.Update(state, 0.016f);

        Assert.True(state.PlayerPaddle.X >= halfWidth);
        Assert.True(state.PlayerPaddle.X <= state.Arena.Width - halfWidth);
    }

    [Fact]
    public void InputSystem_ClampsRightEdge()
    {
        var state = new GameState();
        state.Initialize(800f, 600f);
        state.PlayerPaddle.X = 400f;
        float halfWidth = state.PlayerPaddle.Width / 2;

        var inputSystem = new InputSystem { TargetX = 1000f };
        inputSystem.Update(state, 0.016f);

        Assert.True(state.PlayerPaddle.X <= state.Arena.Width - halfWidth);
    }

    [Fact]
    public void InputSystem_DoesNothingWhenTargetXIsNull()
    {
        var state = new GameState();
        state.Initialize(800f, 600f);
        state.PlayerPaddle.X = 400f;

        var inputSystem = new InputSystem { TargetX = null };
        inputSystem.Update(state, 0.016f);

        Assert.Equal(400f, state.PlayerPaddle.X);
    }

    [Fact]
    public void InputSystem_DoesNothingWhenPaused()
    {
        var state = new GameState();
        state.Initialize(800f, 600f);
        state.IsPaused = true;
        state.PlayerPaddle.X = 400f;

        var inputSystem = new InputSystem { TargetX = 300f };
        inputSystem.Update(state, 0.016f);

        Assert.Equal(400f, state.PlayerPaddle.X);
    }

    [Fact]
    public void InputSystem_DoesNothingWhenNotRunning()
    {
        var state = new GameState();
        state.Initialize(800f, 600f);
        state.IsRunning = false;
        state.PlayerPaddle.X = 400f;

        var inputSystem = new InputSystem { TargetX = 300f };
        inputSystem.Update(state, 0.016f);

        Assert.Equal(400f, state.PlayerPaddle.X);
    }

    [Fact]
    public void InputSystem_ReachesTargetWhenDeltaTimeLarge()
    {
        var state = new GameState();
        state.Initialize(800f, 600f);
        state.PlayerPaddle.X = 100f;

        var inputSystem = new InputSystem { TargetX = 200f };
        inputSystem.Update(state, 1f); // Large delta time

        Assert.Equal(200f, state.PlayerPaddle.X);
    }

    [Fact]
    public void AISystem_MovesOpponentPaddleTowardBallWhenBallMovesUpward()
    {
        var state = new GameState();
        state.Initialize(800f, 600f);
        state.Ball.X = 300f;
        state.Ball.VelocityY = -200f; // Moving upward (toward AI)
        state.OpponentPaddle.X = 100f;

        var aiSystem = new AISystem { Difficulty = 1f }; // 100% accuracy
        aiSystem.Update(state, 0.1f);

        Assert.True(state.OpponentPaddle.X > 100f);
    }

    [Fact]
    public void AISystem_DoesNotMoveWhenBallMovesDownward()
    {
        var state = new GameState();
        state.Initialize(800f, 600f);
        state.Ball.X = 300f;
        state.Ball.VelocityY = 200f; // Moving downward (away from AI)
        state.OpponentPaddle.X = 100f;

        var aiSystem = new AISystem();
        aiSystem.Update(state, 0.1f);

        Assert.Equal(100f, state.OpponentPaddle.X);
    }

    [Fact]
    public void AISystem_DoesNotMoveWhenBallIsStationary()
    {
        var state = new GameState();
        state.Initialize(800f, 600f);
        state.Ball.X = 300f;
        state.Ball.VelocityY = 0f;
        state.OpponentPaddle.X = 100f;

        var aiSystem = new AISystem();
        aiSystem.Update(state, 0.1f);

        Assert.Equal(100f, state.OpponentPaddle.X);
    }

    [Fact]
    public void AISystem_DoesNothingWhenPaused()
    {
        var state = new GameState();
        state.Initialize(800f, 600f);
        state.IsPaused = true;
        state.Ball.VelocityY = -200f;
        state.OpponentPaddle.X = 100f;

        var aiSystem = new AISystem();
        aiSystem.Update(state, 0.1f);

        Assert.Equal(100f, state.OpponentPaddle.X);
    }

    [Fact]
    public void AISystem_DoesNothingWhenNotRunning()
    {
        var state = new GameState();
        state.Initialize(800f, 600f);
        state.IsRunning = false;
        state.Ball.VelocityY = -200f;
        state.OpponentPaddle.X = 100f;

        var aiSystem = new AISystem();
        aiSystem.Update(state, 0.1f);

        Assert.Equal(100f, state.OpponentPaddle.X);
    }

    [Fact]
    public void AISystem_RespectsArenaBoundaries()
    {
        var state = new GameState();
        state.Initialize(800f, 600f);
        state.Ball.X = -100f; // Far left (out of bounds target)
        state.Ball.VelocityY = -200f;
        state.OpponentPaddle.X = 400f;
        float halfWidth = state.OpponentPaddle.Width / 2;

        var aiSystem = new AISystem { Difficulty = 1f };
        aiSystem.Update(state, 0.1f);

        Assert.True(state.OpponentPaddle.X >= halfWidth);
        Assert.True(state.OpponentPaddle.X <= state.Arena.Width - halfWidth);
    }

    [Fact]
    public void AISystem_ResetClearsReactionTimer()
    {
        var state = new GameState();
        state.Initialize(800f, 600f);
        state.Ball.X = 400f;
        state.Ball.VelocityY = -200f;
        state.OpponentPaddle.X = 100f;

        var aiSystem = new AISystem();
        
        // First update with small deltaTime (won't pass reaction delay of 0.05f)
        aiSystem.Update(state, 0.01f);
        float xAfterSmallDelta = state.OpponentPaddle.X;

        // Reset the timer
        aiSystem.Reset();

        // Now a larger deltaTime should cause movement (exceeds 0.05f threshold)
        aiSystem.Update(state, 0.1f);
        float xAfterReset = state.OpponentPaddle.X;

        Assert.Equal(100f, xAfterSmallDelta); // No movement before reset
        Assert.True(xAfterReset > 100f); // Movement after reset
    }

    [Fact]
    public void AISystem_RespectsDifficultyMultiplier()
    {
        var state = new GameState();
        state.Initialize(800f, 600f);
        state.Ball.X = 400f;
        state.Ball.VelocityY = -200f;

        // Test with easy difficulty (0.3)
        var easyAI = new AISystem { Difficulty = 0.3f };
        state.OpponentPaddle.X = 100f;
        easyAI.Update(state, 0.1f);
        float easyX = state.OpponentPaddle.X;

        // Test with hard difficulty (1.0)
        var hardAI = new AISystem { Difficulty = 1f };
        state.OpponentPaddle.X = 100f;
        hardAI.Update(state, 0.1f);
        float hardX = state.OpponentPaddle.X;

        // Hard AI should move faster than easy AI
        Assert.True(hardX > easyX);
    }

    [Fact]
    public void InputSystem_MovesPaddleLeftCorrectly()
    {
        var state = new GameState();
        state.Initialize(800f, 600f);
        state.PlayerPaddle.X = 500f;

        var inputSystem = new InputSystem { TargetX = 200f };
        inputSystem.Update(state, 0.016f);

        Assert.True(state.PlayerPaddle.X < 500f);
        Assert.True(state.PlayerPaddle.X >= 200f);
    }

    [Fact]
    public void AISystem_AddsImprecisionBasedOnDifficulty()
    {
        var state = new GameState();
        state.Initialize(800f, 600f);
        state.Ball.X = 400f;
        state.Ball.VelocityY = -200f;

        var easyAI = new AISystem { Difficulty = 0.2f };
        
        // Run multiple updates to get imprecision variation
        var xPositions = new List<float>();
        for (int i = 0; i < 10; i++)
        {
            state.OpponentPaddle.X = 400f;
            easyAI.Reset();
            easyAI.Update(state, 0.1f);
            xPositions.Add(state.OpponentPaddle.X);
        }

        // With low difficulty (high imprecision), X positions should vary
        var minX = xPositions.Min();
        var maxX = xPositions.Max();
        Assert.True(maxX - minX > 5f); // Meaningful variation
    }
}
