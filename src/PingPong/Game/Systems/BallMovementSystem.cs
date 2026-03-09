namespace PingPong.Game.Systems;

using PingPong.Game.Core;

public class BallMovementSystem
{
    private const float InitialSpeed = 300f;
    private const float SpeedIncrease = 15f;
    private const float MaxSpeed = 600f;

    public void Update(GameState state, float deltaTime)
    {
        if (state.IsPaused || !state.IsRunning) return;

        var ball = state.Ball;
        ball.X += ball.VelocityX * deltaTime;
        ball.Y += ball.VelocityY * deltaTime;

        // Wall collision (left/right bounce)
        if (ball.X - ball.Radius <= 0)
        {
            ball.X = ball.Radius;
            ball.VelocityX = Math.Abs(ball.VelocityX);
        }
        else if (ball.X + ball.Radius >= state.Arena.Width)
        {
            ball.X = state.Arena.Width - ball.Radius;
            ball.VelocityX = -Math.Abs(ball.VelocityX);
        }
    }

    public void LaunchBall(GameState state, bool towardsPlayer = false)
    {
        var ball = state.Ball;
        float angle = (Random.Shared.NextSingle() * 0.5f + 0.25f) * MathF.PI; // 45-135 degrees
        float speed = InitialSpeed;

        ball.VelocityX = MathF.Cos(angle) * speed * (Random.Shared.Next(2) == 0 ? 1 : -1);
        ball.VelocityY = MathF.Sin(angle) * speed * (towardsPlayer ? 1 : -1);
    }

    public void IncreaseSpeed(Ball ball)
    {
        float currentSpeed = MathF.Sqrt(ball.VelocityX * ball.VelocityX + ball.VelocityY * ball.VelocityY);
        if (currentSpeed >= MaxSpeed) return;

        float newSpeed = MathF.Min(currentSpeed + SpeedIncrease, MaxSpeed);
        float factor = newSpeed / currentSpeed;
        ball.VelocityX *= factor;
        ball.VelocityY *= factor;
    }
}
