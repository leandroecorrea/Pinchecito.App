namespace Pinchecito.Core.Interfaces
{
    public record District
    {
        public List<Court> Courts { get; set; }
        public string CreationCode { get; set; }
        public string DistrictName { get; set; }
        public string TrackingCode { get; set; }

        public District(string creationCode, string districtName = "", string trackingCode = "")
        {
            CreationCode = creationCode;
            DistrictName = districtName;
            TrackingCode = trackingCode;
        }
    }
}