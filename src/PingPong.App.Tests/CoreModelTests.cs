using PingPong.Game.Core;

namespace PingPong.App.Tests;

public class CoreModelTests
{
    [Fact]
    public void Ball_Reset_SetsPositionAndZeroesVelocity()
    {
        var ball = new Ball { VelocityX = 5f, VelocityY = -3f };

        ball.Reset(100f, 200f);

        Assert.Equal(100f, ball.X);
        Assert.Equal(200f, ball.Y);
        Assert.Equal(0f, ball.VelocityX);
        Assert.Equal(0f, ball.VelocityY);
    }

    [Fact]
    public void ScoreState_IsGameOver_TrueWhenPlayerReachesWinningScore()
    {
        var score = new ScoreState { PlayerScore = 5 };

        Assert.True(score.IsGameOver);
    }

    [Fact]
    public void ScoreState_IsGameOver_TrueWhenOpponentReachesWinningScore()
    {
        var score = new ScoreState { OpponentScore = 5 };

        Assert.True(score.IsGameOver);
    }

    [Fact]
    public void ScoreState_IsGameOver_FalseWhenNeitherReachesWinningScore()
    {
        var score = new ScoreState { PlayerScore = 3, OpponentScore = 4 };

        Assert.False(score.IsGameOver);
    }

    [Fact]
    public void ScoreState_Reset_ZeroesBothScores()
    {
        var score = new ScoreState { PlayerScore = 3, OpponentScore = 4 };

        score.Reset();

        Assert.Equal(0, score.PlayerScore);
        Assert.Equal(0, score.OpponentScore);
    }

    [Fact]
    public void GameState_Initialize_PositionsPaddlesCorrectly()
    {
        var state = new GameState();

        state.Initialize(800f, 600f);

        Assert.Equal(400f, state.PlayerPaddle.X);
        Assert.Equal(600f - 30f, state.PlayerPaddle.Y);   // arenaHeight - PaddleMargin
        Assert.Equal(400f, state.OpponentPaddle.X);
        Assert.Equal(30f, state.OpponentPaddle.Y);         // PaddleMargin
    }

    [Fact]
    public void GameState_Initialize_ResetsBallToCenter()
    {
        var state = new GameState();

        state.Initialize(800f, 600f);

        Assert.Equal(400f, state.Ball.X);
        Assert.Equal(300f, state.Ball.Y);
        Assert.Equal(0f, state.Ball.VelocityX);
        Assert.Equal(0f, state.Ball.VelocityY);
    }

    [Fact]
    public void GameState_Initialize_SetsIsRunningAndClearsIsPaused()
    {
        var state = new GameState { IsPaused = true };

        state.Initialize(800f, 600f);

        Assert.True(state.IsRunning);
        Assert.False(state.IsPaused);
    }

    [Fact]
    public void GameState_ResetBall_PutsBallAtArenaCenter()
    {
        var state = new GameState();
        state.Arena.Width = 400f;
        state.Arena.Height = 300f;
        state.Ball.VelocityX = 10f;
        state.Ball.VelocityY = -5f;

        state.ResetBall();

        Assert.Equal(200f, state.Ball.X);
        Assert.Equal(150f, state.Ball.Y);
        Assert.Equal(0f, state.Ball.VelocityX);
        Assert.Equal(0f, state.Ball.VelocityY);
    }

    [Fact]
    public void Arena_DefaultPaddleMargin_Is30()
    {
        var arena = new Arena();

        Assert.Equal(30f, arena.PaddleMargin);
    }
}
