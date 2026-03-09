namespace PingPong.App.Views;

using PingPong.App.ViewModels;

public partial class GamePage : ContentPage
{
    private readonly GameDrawable _drawable;
    private readonly GameViewModel _viewModel;

    public GamePage()
    {
        InitializeComponent();
        _drawable = new GameDrawable();
        _viewModel = new GameViewModel();

        GameCanvas.Drawable = _drawable;
        BindingContext = _viewModel;
    }

    public GameDrawable Drawable => _drawable;
    public GameViewModel ViewModel => _viewModel;
}
