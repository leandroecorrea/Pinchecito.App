using Pinchecito.UI.ViewModel;

namespace Pinchecito.UI.Views;

public partial class HomePage : ContentPage
{
	private HomeViewModel _viewModel;
    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        await _viewModel.GetUserData();
        base.OnNavigatedTo(args);
    }


    private async void OnNavigateToTrackFile(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(TrackFilePage));
    }
}