namespace PingPong.Game.Core;

public class Paddle
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Width { get; set; } = 80f;
    public float Height { get; set; } = 15f;
    public float Speed { get; set; } = 400f;

    public float Left => X - Width / 2;
    public float Right => X + Width / 2;
    public float Top => Y - Height / 2;
    public float Bottom => Y + Height / 2;
}
