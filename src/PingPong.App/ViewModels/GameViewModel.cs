namespace PingPong.App.ViewModels;

using System.ComponentModel;
using System.Runtime.CompilerServices;

public class GameViewModel : INotifyPropertyChanged
{
    private int _playerScore;
    private int _opponentScore;
    private bool _isGameOver;
    private bool _isPaused;
    private string _gameOverText = string.Empty;

    public int PlayerScore
    {
        get => _playerScore;
        set { _playerScore = value; OnPropertyChanged(); }
    }

    public int OpponentScore
    {
        get => _opponentScore;
        set { _opponentScore = value; OnPropertyChanged(); }
    }

    public bool IsGameOver
    {
        get => _isGameOver;
        set { _isGameOver = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsPlaying)); }
    }

    public bool IsPaused
    {
        get => _isPaused;
        set { _isPaused = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsPlaying)); }
    }

    public bool IsPlaying => !IsGameOver && !IsPaused;

    public string GameOverText
    {
        get => _gameOverText;
        set { _gameOverText = value; OnPropertyChanged(); }
    }

    public void UpdateFromScore(int playerScore, int opponentScore, bool isGameOver)
    {
        PlayerScore = playerScore;
        OpponentScore = opponentScore;
        IsGameOver = isGameOver;
        if (isGameOver)
        {
            GameOverText = playerScore > opponentScore ? "You Win!" : "You Lose!";
        }
    }

    public void Reset()
    {
        PlayerScore = 0;
        OpponentScore = 0;
        IsGameOver = false;
        IsPaused = false;
        GameOverText = string.Empty;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
