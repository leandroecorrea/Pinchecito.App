using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pinchecito.Core.Interfaces;
using System.Collections.ObjectModel;

namespace Pinchecito.UI.ViewModel
{
    public partial class TrackFileViewModel : ObservableObject
    {
        [ObservableProperty] ObservableCollection<QueryDistrict> availableDistricts;        
        [ObservableProperty] QueryDistrict selectedDistrict;
        [ObservableProperty] Court selectedCourt;
        [ObservableProperty] string searchText;
        [ObservableProperty] ObservableCollection<TrackableFile> trackableFiles;
        [ObservableProperty] TrackableFile selectedTrackableFile;
        [ObservableProperty] ObservableCollection<Court> courts;
        [ObservableProperty] bool dataWasRetrieved;
        [ObservableProperty] bool dataIsLoading;
        [ObservableProperty] bool isSearching;
        [ObservableProperty] string lastUpdateString;
        
        private readonly ITrackFileService _trackFileService;


        public TrackFileViewModel(ITrackFileService trackFileService)
        {
            _trackFileService = trackFileService;
            availableDistricts = new();            
            trackableFiles = new();     
            courts = new();
            dataWasRetrieved= false;
            dataIsLoading = true;
        }

        public async Task GetDataForRequest()
        {
            if (!AvailableDistricts.Any())
            {
                var districts = await _trackFileService.GetDistrictsForTracking();
                if (districts.IsSuccess)
                {
                    AvailableDistricts = new(districts.Value.ToList());                    
                    SelectedDistrict = AvailableDistricts.FirstOrDefault();
                    Courts = new(SelectedDistrict.Courts.ToList());  
                    SelectedCourt = Courts.FirstOrDefault();
                    DataWasRetrieved = true;
                    DataIsLoading= false;
                }
            }
        }

       

        [RelayCommand]
        public async Task Search()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                return;
            IsSearching = true;
            TrackableFiles.Clear();
            var request = new GetTrackableFileByIdRequest(SelectedDistrict.QueryCode, SelectedCourt.QueryCode, SearchText.Trim());
            var track = await _trackFileService.GetTrackablesFor(request);
            if (track.IsSuccess)
            {                
                TrackableFiles = new(track.Value);                
            }
            else
            {

            }
            IsSearching= false;
        }

        [RelayCommand]
        public async Task Track()
        {
            
        }

        public void UpdateCourt()
        {            
            if(SelectedDistrict != null)
                Courts = new(SelectedDistrict.Courts);
        }
    }
}
