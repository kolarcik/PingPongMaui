namespace PingPong.App.Game.Systems;

using PingPong.App.Game.Core;

public class AISystem
{
    public float Difficulty { get; set; } = 0.7f; // 0.0 to 1.0
    private const float ReactionDelay = 0.05f;
    private float _reactionTimer;

    public void Update(GameState state, float deltaTime)
    {
        if (state.IsPaused || !state.IsRunning) return;

        _reactionTimer += deltaTime;
        if (_reactionTimer < ReactionDelay) return;
        _reactionTimer = 0;

        var paddle = state.OpponentPaddle;
        var ball = state.Ball;

        // Only track ball when it's moving towards the AI (upward)
        if (ball.VelocityY >= 0) return;

        float target = ball.X;

        // Add imprecision based on difficulty
        float error = (1f - Difficulty) * paddle.Width;
        target += (Random.Shared.NextSingle() - 0.5f) * error;

        // Clamp within arena
        float halfWidth = paddle.Width / 2;
        target = Math.Clamp(target, halfWidth, state.Arena.Width - halfWidth);

        // Move paddle
        float diff = target - paddle.X;
        float maxMove = paddle.Speed * Difficulty * deltaTime;

        if (Math.Abs(diff) <= maxMove)
            paddle.X = target;
        else
            paddle.X += Math.Sign(diff) * maxMove;
    }

    public void Reset()
    {
        _reactionTimer = 0;
    }
}
