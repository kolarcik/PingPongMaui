namespace PingPong.App.Game.Core;

public class ScoreState
{
    public int PlayerScore { get; set; }
    public int OpponentScore { get; set; }
    public int WinningScore { get; set; } = 5;
    public bool IsGameOver => PlayerScore >= WinningScore || OpponentScore >= WinningScore;

    public void Reset()
    {
        PlayerScore = 0;
        OpponentScore = 0;
    }
}
