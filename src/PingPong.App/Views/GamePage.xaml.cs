namespace PingPong.App.Views;

public partial class GamePage : ContentPage
{
    private readonly GameDrawable _drawable;

    public GamePage()
    {
        InitializeComponent();
        _drawable = new GameDrawable();
        GameCanvas.Drawable = _drawable;
    }

    public GameDrawable Drawable => _drawable;
}
