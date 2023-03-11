using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pinchecito.Core.Interfaces;
using System.Collections.ObjectModel;

namespace Pinchecito.UI.ViewModel
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly ILoginService _loginService;
        [ObservableProperty] ObservableCollection<CreationDistrict> availableDistricts;
        [ObservableProperty] CreationDistrict selectedDistrict;
        [ObservableProperty] string password;
        [ObservableProperty] string username;
        [ObservableProperty] bool isLoginBeingProcessed;
        [ObservableProperty] string loginError;
        [ObservableProperty] bool loginFailure;
        public event Action OnLoginSuccessfull;

        public LoginViewModel(ILoginService loginService)
        {
            _loginService = loginService;
            availableDistricts = new();
            isLoginBeingProcessed = false;
            loginError = string.Empty;
            loginFailure = false;
        }


        public async Task GetDistricts()
        {
            var districts = await _loginService.GetCreationDistricts();
            if (!districts.IsSuccess) return;
            AvailableDistricts = new();
            foreach (var district in districts.Value)
            {
                AvailableDistricts.Add(district);
            }
        }

        [RelayCommand]
        public async void Login()
        {            
            if (LoginFormIsNotValid())
                return;
            IsLoginBeingProcessed = true;
            var loginResult = await _loginService.Login(new(Username, Password, SelectedDistrict.CreationCode));
            IsLoginBeingProcessed = false;
            if (loginResult.IsSuccess)
            {
                LoginFailure = false;
                OnLoginSuccessfull?.Invoke();
            }
            else
            {
                LoginFailure = true;
                LoginError = $"Error en el login. Error:{loginResult.Errors.FirstOrDefault()}";
            }
        }

        private bool LoginFormIsNotValid() => 
            string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) || SelectedDistrict == null;        
    }
}
