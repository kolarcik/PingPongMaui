namespace PingPong.Game.Core;

public class Ball
{
    public float X { get; set; }
    public float Y { get; set; }
    public float VelocityX { get; set; }
    public float VelocityY { get; set; }
    public float Radius { get; set; } = 10f;

    public void Reset(float x, float y)
    {
        X = x;
        Y = y;
        VelocityX = 0;
        VelocityY = 0;
    }
}
