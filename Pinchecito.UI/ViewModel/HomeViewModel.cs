using CommunityToolkit.Mvvm.ComponentModel;
using Pinchecito.Core.Interfaces;

namespace Pinchecito.UI.ViewModel
{
    public partial class HomeViewModel : ObservableObject
    {
        [ObservableProperty] private string username;
        [ObservableProperty] private string fullname;
        [ObservableProperty] private int filesBeingTracked;
        [ObservableProperty] private DateTime lastNotification;
        private readonly IUserRepository _userRepository;

        public HomeViewModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task GetUserData()
        {
            var data = await _userRepository.Get();
            if(data.IsSuccess)
            {
                Username = data.Value.Username;
                Fullname = data.Value.Fullname;
                FilesBeingTracked = data.Value.TrackedFiles.Count();
                LastNotification = data.Value.TrackedFiles
                                             .OrderByDescending(x=> x.LastOrder)
                                             .FirstOrDefault()?.LastOrder.Date ??                                                   DateTime.Now;
            }
        }
    }
}
