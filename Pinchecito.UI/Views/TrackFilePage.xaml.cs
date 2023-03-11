using Pinchecito.UI.ViewModel;

namespace Pinchecito.UI.Views;

public partial class TrackFilePage : ContentPage
{
    private TrackFileViewModel _viewModel;

    public TrackFilePage(TrackFileViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
        _viewModel = viewModel;
	}

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        await _viewModel.GetDataForRequest();
        base.OnNavigatedTo(args);
    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        _viewModel.UpdateCourt();
    }
}