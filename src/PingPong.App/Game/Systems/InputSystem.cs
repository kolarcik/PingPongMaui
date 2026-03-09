namespace PingPong.App.Game.Systems;

using PingPong.App.Game.Core;

public class InputSystem
{
    public float? TargetX { get; set; }

    public void Update(GameState state, float deltaTime)
    {
        if (state.IsPaused || !state.IsRunning) return;
        if (TargetX == null) return;

        var paddle = state.PlayerPaddle;
        float target = TargetX.Value;

        // Clamp target within arena bounds
        float halfWidth = paddle.Width / 2;
        target = Math.Clamp(target, halfWidth, state.Arena.Width - halfWidth);

        // Move paddle towards target
        float diff = target - paddle.X;
        float maxMove = paddle.Speed * deltaTime;

        if (Math.Abs(diff) <= maxMove)
            paddle.X = target;
        else
            paddle.X += Math.Sign(diff) * maxMove;
    }
}
