namespace PingPong.App.Game.Systems;

using PingPong.App.Game.Core;

public class CollisionSystem
{
    private readonly BallMovementSystem _ballMovement;

    public CollisionSystem(BallMovementSystem ballMovement)
    {
        _ballMovement = ballMovement;
    }

    public bool Update(GameState state)
    {
        if (state.IsPaused || !state.IsRunning) return false;

        var ball = state.Ball;
        bool scored = false;
        bool opponentScored = false;

        // Check paddle collisions
        if (CheckPaddleCollision(ball, state.PlayerPaddle))
        {
            ball.VelocityY = -Math.Abs(ball.VelocityY); // bounce up
            AdjustAngle(ball, state.PlayerPaddle);
            _ballMovement.IncreaseSpeed(ball);
        }
        else if (CheckPaddleCollision(ball, state.OpponentPaddle))
        {
            ball.VelocityY = Math.Abs(ball.VelocityY); // bounce down
            AdjustAngle(ball, state.OpponentPaddle);
            _ballMovement.IncreaseSpeed(ball);
        }

        // Check scoring (ball passed top or bottom)
        if (ball.Y - ball.Radius <= 0)
        {
            state.Score.PlayerScore++;
            scored = true;
            opponentScored = false;
        }
        else if (ball.Y + ball.Radius >= state.Arena.Height)
        {
            state.Score.OpponentScore++;
            scored = true;
            opponentScored = true;
        }

        if (scored)
        {
            if (state.Score.IsGameOver)
            {
                state.IsRunning = false;
            }
            else
            {
                // Capture direction before ResetBall moves ball to center
                state.ResetBall();
                _ballMovement.LaunchBall(state, towardsPlayer: opponentScored);
            }
        }

        return scored;
    }

    private bool CheckPaddleCollision(Ball ball, Paddle paddle)
    {
        float paddleLeft = paddle.X - paddle.Width / 2;
        float paddleRight = paddle.X + paddle.Width / 2;
        float paddleTop = paddle.Y - paddle.Height / 2;
        float paddleBottom = paddle.Y + paddle.Height / 2;

        return ball.X + ball.Radius >= paddleLeft &&
               ball.X - ball.Radius <= paddleRight &&
               ball.Y + ball.Radius >= paddleTop &&
               ball.Y - ball.Radius <= paddleBottom;
    }

    private void AdjustAngle(Ball ball, Paddle paddle)
    {
        // Offset from paddle center (-1 to 1)
        float offset = (ball.X - paddle.X) / (paddle.Width / 2);
        offset = Math.Clamp(offset, -1f, 1f);

        float speed = MathF.Sqrt(ball.VelocityX * ball.VelocityX + ball.VelocityY * ball.VelocityY);
        float maxAngle = MathF.PI / 3; // 60 degrees max
        float angle = offset * maxAngle;

        ball.VelocityX = MathF.Sin(angle) * speed;
        ball.VelocityY = MathF.Cos(angle) * speed * MathF.Sign(ball.VelocityY);
    }
}
