using Pinchecito.UI.ViewModel;
using Pinchecito.UI.Views;

namespace Pinchecito.UI;

public partial class MainPage : ContentPage
{
    private MainViewModel _viewModel;

    public MainPage(MainViewModel viewModel)
	{
        _viewModel = viewModel;
		InitializeComponent();
	}

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (await _viewModel.VerifyLoginStatus())
        {
            await Shell.Current.GoToAsync(nameof(HomePage));
        }
        else
        {
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }
        base.OnNavigatedTo(args);
    }
}

