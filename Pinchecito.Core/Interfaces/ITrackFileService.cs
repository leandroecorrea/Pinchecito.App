namespace Pinchecito.Core.Interfaces
{
    public interface ITrackFileService
    {
        Task<Result<TrackedFile>> TrackFile(GetTrackedFilesRequest request);
        Task<Result<IEnumerable<TrackableFile>>> GetTrackablesFor(GetTrackableFileByIdRequest request);
        Task<Result<IEnumerable<QueryDistrict>>> GetDistrictsForTracking();        
    }
}
