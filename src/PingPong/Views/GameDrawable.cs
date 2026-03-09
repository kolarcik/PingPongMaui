namespace PingPong.Views;

using PingPong.Game.Core;

public class GameDrawable : IDrawable
{
    public GameState? GameState { get; set; }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if (GameState == null) return;

        DrawArena(canvas, dirtyRect);
        DrawBall(canvas);
        DrawPaddle(canvas, GameState.PlayerPaddle, Colors.White);
        DrawPaddle(canvas, GameState.OpponentPaddle, Colors.LightGray);
        DrawCenterLine(canvas, dirtyRect);
    }

    private void DrawArena(ICanvas canvas, RectF dirtyRect)
    {
        canvas.FillColor = Colors.Black;
        canvas.FillRectangle(dirtyRect);
    }

    private void DrawBall(ICanvas canvas)
    {
        var ball = GameState!.Ball;
        canvas.FillColor = Colors.White;
        canvas.FillCircle(ball.X, ball.Y, ball.Radius);
    }

    private void DrawPaddle(ICanvas canvas, Paddle paddle, Color color)
    {
        canvas.FillColor = color;
        float left = paddle.X - paddle.Width / 2;
        float top = paddle.Y - paddle.Height / 2;
        canvas.FillRoundedRectangle(left, top, paddle.Width, paddle.Height, 4);
    }

    private void DrawCenterLine(ICanvas canvas, RectF dirtyRect)
    {
        canvas.StrokeColor = Colors.Gray;
        canvas.StrokeSize = 2;
        canvas.StrokeDashPattern = new float[] { 10, 5 };
        float centerY = dirtyRect.Height / 2;
        canvas.DrawLine(0, centerY, dirtyRect.Width, centerY);
    }
}
