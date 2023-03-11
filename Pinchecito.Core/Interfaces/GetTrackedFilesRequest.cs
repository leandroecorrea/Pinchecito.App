namespace Pinchecito.Core.Interfaces
{
    public record GetTrackedFilesRequest(string DistrictCode,
                                   string Jurisdiction,
                                   string Court,
                                   int SearchOption,
                                   string SearchQuery,
                                   DateTime From,
                                   DateTime To);

    public record GetTrackableFileByIdRequest(string QueryDistrict,
                                       string CourtId,
                                       string FileNumber);

    public record TrackFileRequest(District District, TrackableFile FileToBeTracked);
}
