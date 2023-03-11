using Pinchecito.UI.ViewModel;
using Pinchecito.UI.Views;

namespace Pinchecito.UI;

public partial class LoginPage : ContentPage
{
    private LoginViewModel _viewModel;

    public LoginPage(LoginViewModel loginViewModel)
    {
        InitializeComponent();
        _viewModel = loginViewModel;
        BindingContext = loginViewModel;
        _viewModel.OnLoginSuccessfull += NavigateToHome;
    }
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        await _viewModel.GetDistricts();
        base.OnNavigatedTo(args);
    }

    private async void NavigateToHome()
    {
        await Shell.Current.GoToAsync(nameof(HomePage));
    }

}